export interface ILoginResponse {
  userId  : string;
  username: string;
  token   : string;
}

export interface ILoginRequest {
  username: string;
  password: string;
}