using GroupLibraryManagment.Webjobs.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupLibraryManagment.Webjobs
{
    public class Functions
    {
        private readonly GroupLibraryManagmentDbContext _dbContext;

        public Functions(GroupLibraryManagmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task RequestEnddateStatus(
            [TimerTrigger("0 1 0 * * *")] TimerInfo timerInfo,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("RequestEnddateEminent function started.");

            // Now you can use _dbContext to interact with the database
            var requests = await _dbContext.Requests
                .Where(r => r.RequestStatus == RequestStatus.Requested)
                .ToListAsync();

            foreach (var request in requests)
            {
                // Check if EndDate is null
                if (request.EndDate.HasValue)
                {
                    // Calculate the difference in days between EndDate and DateTime.Now
                    TimeSpan difference = request.EndDate.Value - DateTime.Now;
                    int daysDifference = (int)difference.TotalDays;

                    // Check if the difference is less than or equal to 3 days
                    if (daysDifference <= 3 && daysDifference >= 0)
                    {
                        // Send notification to request
                        var notification = Notification.CreateNotificationForEndRequestEminent(request.RequestId, request.ISBN, DateTime.Now, daysDifference);

                        _dbContext.Notifications.Add(notification);
                        await _dbContext.SaveChangesAsync();

                    }
                    else if (daysDifference < 0)
                    {
                        if (request.Punishment == null)
                        {
                            var punishment = Punishment.CreatePunishmentForRequest(request.RequestId);
                            _dbContext.Punishments.Add(punishment);
                            // Send notification to request
                            var notification = Notification.CreateNotificationForPunishment(request.RequestId, request.ISBN, punishment.PunishmentReason);
                            _dbContext.Notifications.Add(notification);
                            await _dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            if (request.Punishment.PunishmentLevel < PunishmentLevel.Five)
                            {
                                var punishment = Punishment.IncreasePunishmentLevel(request.Punishment);
                                _dbContext.Punishments.Update(punishment);
                                var notification = Notification.CreateNotificationForIncreasePunishmentLevel(request.RequestId, request.ISBN, punishment.PunishmentLevel);
                                _dbContext.Notifications.Add(notification);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
            }

            logger.LogInformation("RequestEnddateEminent function completed.");
        }

        public async Task RequestPendingEnd(
            [TimerTrigger("0 1 0 * * *")] TimerInfo timerInfo,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("RequestPendingEnd function started.");

            // Now you can use _dbContext to interact with the database
            var requests = await _dbContext.Requests
                .Where(r => r.RequestStatus == RequestStatus.Pending)
                .ToListAsync();

            foreach (var request in requests)
            {
                if (request.EndDate.HasValue)
                {
                    // Calculate the difference in days between EndDate and DateTime.Now
                    TimeSpan difference = request.EndDate.Value - DateTime.Now;
                    int daysDifference = (int)difference.TotalDays;

                    if (daysDifference < 0)
                    {
                        // Change request status to canceled
                        request.RequestStatus = RequestStatus.Canceled;
                        // Change physical book status to available
                        var physicalBook = await _dbContext.PhysicalBooks.FirstOrDefaultAsync(pb => pb.PhysicalBookId == request.PhysicalBookId);
                        physicalBook.PhysicalBookStatus = PhysicalBookStatus.AtLibrary;

                        // Send notification to request
                        var notification = Notification.CreateNotificationForCanceledRequest(request.RequestId, request.ISBN);

                        _dbContext.Notifications.Add(notification);
                        _dbContext.Requests.Update(request);
                        _dbContext.PhysicalBooks.Update(physicalBook);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }

            logger.LogInformation("RequestPendingEnd function completed.");
        }

        public async Task TransferEnding(
            [TimerTrigger("0 1 0 * * *")] TimerInfo timerInfo,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("TransferEnding function started.");

            // Now you can use _dbContext to interact with the database
            var transfers = await _dbContext.Transfers
                .Where(t => t.TransferStatus == TransferStatus.Pending)
                .ToListAsync();

            foreach (var transfer in transfers)
            {
                // Calculate the difference in days between EndDate and DateTime.Now
                TimeSpan difference = transfer.EndDate - DateTime.Now;
                int daysDifference = (int)difference.TotalDays;

                if (daysDifference < 0)
                {
                    // Change transfer status to canceled
                    transfer.TransferStatus = TransferStatus.Canceled;
                    // Change physical book status to at library
                    var physicalBook = await _dbContext.PhysicalBooks.FirstOrDefaultAsync(pb => pb.PhysicalBookId == transfer.PhysicalBookId);
                    physicalBook.PhysicalBookStatus = PhysicalBookStatus.AtLibrary;

                    // Get source library
                    var library = await _dbContext.Libraries.FirstOrDefaultAsync(l => l.LibraryId == transfer.SourceLibraryId);

                    // Send notification to request
                    var notification = Notification.CreateNotificationForCanceledTransfer(transfer.DestinationLibraryId, library.LibraryAlias, transfer.PhysicalBookId);

                    _dbContext.Notifications.Add(notification);
                    _dbContext.Transfers.Update(transfer);
                    _dbContext.PhysicalBooks.Update(physicalBook);
                    await _dbContext.SaveChangesAsync();
                }
            }

            logger.LogInformation("TransferEnding function completed.");
        }

        public async Task CheckIfBookHasAvailableCopies(
            [TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("CheckIfBookHasAvailableCopies function started.");
            var requests = await _dbContext.Requests
                .Where(r => r.RequestStatus == RequestStatus.Waiting)
                .ToListAsync();

            foreach (var request in requests)
            {
                if (_dbContext.PhysicalBooks.Any(pb => pb.ISBN == request.ISBN && pb.PhysicalBookStatus == PhysicalBookStatus.AtLibrary && pb.LibraryId == request.LibraryId))
                {
                    var physycalBook = await _dbContext.PhysicalBooks.FirstOrDefaultAsync(pb => pb.ISBN == request.ISBN && pb.PhysicalBookStatus == PhysicalBookStatus.AtLibrary && pb.LibraryId == request.LibraryId);
                    request.RequestStatus = RequestStatus.Pending;
                    request.PhysicalBookId = physycalBook.PhysicalBookId;
                    physycalBook.PhysicalBookStatus = PhysicalBookStatus.Requested;
                    request.EndDate = DateTime.Now.AddDays(3);

                    // add notification
                    var notification = Notification.CreateNotificationForAvailableBook(request.UserId, request.ISBN);
                    _dbContext.Notifications.Add(notification);
                    _dbContext.PhysicalBooks.Update(physycalBook);
                    _dbContext.Requests.Update(request);
                    await _dbContext.SaveChangesAsync();
                }
            }
            logger.LogInformation("CheckIfBookHasAvailableCopies function completed.");
        }
    }
}
