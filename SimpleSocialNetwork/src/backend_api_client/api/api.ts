export * from './auth-api.service';
import { AuthApiService } from './auth-api.service';
export * from './comment-api.service';
import { CommentApiService } from './comment-api.service';
export * from './opMessage-api.service';
import { OpMessageApiService } from './opMessage-api.service';
export * from './user-api.service';
import { UserApiService } from './user-api.service';
export const APIS = [AuthApiService, CommentApiService, OpMessageApiService, UserApiService];
