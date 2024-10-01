import { REQUEST_STATUSES } from '../constants/constants';
import RequestState from '@/enums/requestStateEnum';

export const getStateText = (state: number) => {
  switch (state) {
    case RequestState.Pending:
      return REQUEST_STATUSES.PENDING;
    case RequestState.Request:
      return REQUEST_STATUSES.REQUEST;
    case RequestState.NotReturned:
      return REQUEST_STATUSES.NOT_RETURNED;
    case RequestState.Finished:
      return REQUEST_STATUSES.FINISHED;
    case RequestState.Waiting:
      return REQUEST_STATUSES.WAITING;
    case RequestState.Canceled:
      return REQUEST_STATUSES.CANCELED;
    default:
      return 'Unknown State';
  }
};
// 0-Pending, 1-Request, 2-NotReturned, 3-Finished, 4-Waiting
