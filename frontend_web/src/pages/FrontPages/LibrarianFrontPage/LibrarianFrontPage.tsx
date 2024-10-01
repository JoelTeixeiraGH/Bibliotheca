import { useAuth } from '@/context/AuthContext';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Container, Typography, Paper, Grid, Divider } from '@mui/material';
import transferService from '@/services/transferService';
import requestService from '@/services/requestService';
import { Transfer, TransferStatus } from '@/models/Transfer';
import { Request, RequestStatus } from '@/models/Request';
import { Link } from 'react-router-dom';
import { Button } from 'react-bootstrap';
import GridTable from '@/components/table/GridTable';
import physicalBookService from '@/services/physicalBookService';
import { PhysicalBook } from '@/models/PhysicalBook';
import notificationService from '@/services/notificationService';
import { Notification } from '@/models/Notification';

export const screenName = "pages.LibrarianFrontPage";

function LibrarianFrontPage() {
    const { t } = useTranslation();
    const [date, setDate] = useState<string>("");
    const { user } = useAuth();
    const [pendingTransfers, setPendingTransfers] = useState<number>(0);
    const [pendingRequests, setPendingRequests] = useState<number>(0);
    const [pendingNotifications, setPendingNotifications] = useState<number>(0);
    const [physicalBooks, setPhysicalBooks] = useState<PhysicalBook[]>([]);
    const [update, setUpdate] = useState(false);
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {

        const fetchTransfers = async () => {
            try {
                const response = await transferService.getAllTransfersByLibraryId(user.LibraryId);

                if (response.status === 200) {
                    const allTransfers = response.data;
                    const pendingTransfersCount = allTransfers.filter(
                        (transfer: Transfer) => transfer.transferStatus === TransferStatus.Pending
                    ).length;

                    setPendingTransfers(pendingTransfersCount);
                }

            } catch (error) {
                console.error(error);
            }
        };

        const fetchRequests = async () => {
            try {
                const response = await requestService.getAllRequestsByLibraryId(user.LibraryId);

                if (response.status === 200) {
                    const allRequests: Request[] = response.data;  // Fornecer explicitamente o tipo
                    const pendingRequestsCount = allRequests.filter(
                        (request: Request) => request.requestStatus === RequestStatus.Pending
                    ).length;

                    setPendingRequests(pendingRequestsCount);
                }
            } catch (error) {
                console.error(error);
            }
        };

        const fetchNotifications = async () => {
            try {
                const response = await notificationService.getAllNotificationsByLibraryIdForLibrary(user.LibraryId);

                if (response.status === 200) {
                    const allNotifications: Notification[] = response.data;  // Fornecer explicitamente o tipo
                    const currentDate = new Date();
                    const pendingNotificationsCount = allNotifications.filter((notification: Notification) => {
                        // Assuming notification.endDate is in "dd/mm/yyyy" format
                        const [day, month, year] = notification.endDate.toString().split('/');
                        const endDate = new Date(`${month}/${day}/${year}`);

                        return endDate > currentDate;
                    }).length;

                    setPendingNotifications(pendingNotificationsCount);
                }
            } catch (error) {
                console.error(error);
            }
        };

        const today = new Date();
        setDate(today.getDate() + " " + t('months.' + today.getMonth()) + " " + today.getFullYear());

        fetchNotifications();
        fetchTransfers();
        fetchRequests();

    }, [user.LibraryId]);

    useEffect(() => {
        setIsLoading(true);

        const fetchCopiesTransfer = async () => {
            try {
                const response = await physicalBookService.getAllPhysicalBooksWithTransferStatusForLibrary(user.LibraryId);

                if (response.status === 200) {
                    setPhysicalBooks(response.data);
                } else {
                    setPhysicalBooks([]);
                }

            } catch (error) {
                console.error(error);
            }
        }

        fetchCopiesTransfer();
        setUpdate(false);
        setIsLoading(false);

    }, [update]);

    const handleMenuClose = () => {
        setUpdate(true);
    };

    return (
        <>
            <h1>{t(screenName + ".header.greeting")}, {user.Name}</h1>
            <p>{date}</p>
            {/*<h1>{t(screenName + ".header.library")} {libraryName}, ID {user.LibraryId}</h1>*/}

            <Container style={{ marginTop: '20px' }}>
                <Grid container spacing={3}>
                    <Grid item xs={4}>
                        <Paper elevation={3} style={{ padding: '20px', height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
                            <Typography variant="h5" gutterBottom>
                                {t(screenName + ".dashboard.notifications_pending")}
                            </Typography>
                            <Link to="/transfers" style={{ textDecoration: 'none' }}>
                                <Typography variant="h3" className="bold-black">
                                    {pendingNotifications}
                                </Typography>
                            </Link>
                        </Paper>
                    </Grid>
                    <Grid item xs={4}>
                        <Paper elevation={3} style={{ padding: '20px', height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
                            <Typography variant="h5" gutterBottom>
                                {t(screenName + ".dashboard.transfers_pending")}
                            </Typography>
                            <Link to="/transfers" style={{ textDecoration: 'none' }}>
                                <Typography variant="h3" className="bold-black">
                                    {pendingTransfers}
                                </Typography>
                            </Link>
                        </Paper>
                    </Grid>
                    <Grid item xs={4}>
                        <Paper elevation={3} style={{ padding: '20px', height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
                            <Typography variant="h5" gutterBottom>
                                {t(screenName + ".dashboard.requests_pending")}
                            </Typography>
                            <Link to="/requests" style={{ textDecoration: 'none' }}>
                                <Typography variant="h3" className="bold-black">
                                    {pendingRequests}
                                </Typography>
                            </Link>
                        </Paper>
                    </Grid>
                </Grid>
                <Divider sx={{ my: 2 }} />
                {isLoading ? (
                    <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
                ) : (
                    <>
                        {physicalBooks.length != 0 ? (
                            <>
                                <Typography variant="h5" gutterBottom>
                                    {t(screenName + ".dashboard.transfer_books")}
                                </Typography>
                                <GridTable rows={physicalBooks as []} columnName={'physicalBooks'} onMenuClose={handleMenuClose} />
                            </>
                        ) : (
                            <Typography variant="h5" gutterBottom>
                                {t(screenName + ".dashboard.transfer_books_empty")}
                            </Typography>
                        )}
                    </>
                )}
            </Container>


        </>
    );
}

export default LibrarianFrontPage;
