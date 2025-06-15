using Homeoffice.Contracts.Dtos;
using Homeoffice.Contracts.Services;
using Homeoffice.DataAccess;
using Homeoffice.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Homeoffice.Services
{
    public class TimeTrackingService : ITimeTrackingService
    {
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;
        public TimeTrackingService(
            IEmailService emailService,
            ApplicationDbContext context
            )
        {
            _emailService = emailService;
            _context = context;
        }
        public async Task<HomeOfficeEntryDto> StartTrackingAsync(string userId, string? description = null)
        {
            var isAlreadyTracking = await _context.HomeOfficeEntries
                .AnyAsync(e => e.UserId == userId && e.EndTime == null);

            if (isAlreadyTracking)
            {
                throw new InvalidOperationException("User is already tracking time.");
            }

            var newEntry = new HomeOfficeEntry
            {
                UserId = userId,
                Description = description,
                StartTime = DateTimeOffset.UtcNow,
                IsEmailSent = false
            };

            await _context.HomeOfficeEntries.AddAsync(newEntry);
            await _context.SaveChangesAsync();

            return new HomeOfficeEntryDto
            (
                newEntry.Id,
                newEntry.StartTime,
                newEntry.EndTime,
                newEntry.Description,
                newEntry.IsEmailSent,
                newEntry.UserId
            );
        }
        public async Task<HomeOfficeEntryDto?> StopTrackingAsync(string userId, string? description = null)
        {
            var activeEntry = _context.HomeOfficeEntries
                .Include(e => e.User)
                .FirstOrDefault(e => e.UserId == userId && e.EndTime == null);

            if (activeEntry is null)
            {
                return null;
            }

            activeEntry.EndTime = DateTimeOffset.UtcNow;

            try
            {
                await _emailService.SendEmailAsync(activeEntry);
                activeEntry.IsEmailSent = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send notification email for entry {activeEntry.Id}: {ex}");
            }

            await _context.SaveChangesAsync();

            return new HomeOfficeEntryDto
            (
                activeEntry.Id,
                activeEntry.StartTime,
                activeEntry.EndTime,
                description ?? activeEntry.Description,
                activeEntry.IsEmailSent,
                activeEntry.UserId
            );
        }
        public async Task<IList<HomeOfficeEntryDto>> GetOverviewAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var entries = await _context.HomeOfficeEntries
                .Where(e => e.UserId == userId && e.StartTime >= startDate && e.StartTime <= endDate)
                .OrderByDescending(e => e.StartTime)
                .ToListAsync();

            var dtos = entries.Select(e => new HomeOfficeEntryDto
            (
                e.Id,
                e.StartTime,
                e.EndTime,
                e.Description,
                e.IsEmailSent,
                e.UserId
            )).ToList();

            return dtos;
        }
    }
}
