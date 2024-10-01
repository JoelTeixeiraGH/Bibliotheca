import { Assignment } from '@mui/icons-material';
import { Button, Divider, TextField, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import requestService from '@/services/requestService';
import { Request, RequestStatus } from '@/models/Request';
import GridTable from '@/components/table/GridTable';
import { useAuth } from '@/context/AuthContext';
import { useTranslation } from 'react-i18next';

export const screenName = "pages.RequestsPage."

const RequestsPage = () => {
    const { t } = useTranslation();
    const [data, setData] = useState<Request[]>([]);
    const [filteredData, setFilteredData] = useState<Request[]>([]);
    const [activeRequests, setActiveRequests] = useState<Request[]>([]);
    const [pastRequests, setPastRequests] = useState<Request[]>([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [update, setUpdate] = useState(false);
    const [error, setError] = useState(false);

    const { user } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        setIsLoading(true);

        const fetchData = async () => {
            try {
                const response = await requestService.getAllRequestsByLibraryId(user.LibraryId);

                if (response.status === 200) {
                    setData(response.data);
                    setActiveRequests(response.data.filter((request: Request) => request.requestStatus != RequestStatus.Returned && request.requestStatus != RequestStatus.Canceled));
                    setPastRequests(response.data.filter((request: Request) => request.requestStatus === RequestStatus.Returned || request.requestStatus === RequestStatus.Canceled));
                    setFilteredData(activeRequests);
                }

            } catch (error: any) {
                setError(error.message);
            }
        };

        fetchData();
        setIsLoading(false);
        setUpdate(false);
    }, [update]);

    useEffect(() => {
        // Filter data based on the search query
        if (Array.isArray(activeRequests)) {
            const filteredRequests = activeRequests.filter(request =>
                request.requestId.toString().includes(searchQuery) ||
                request.user.userEmail.toLowerCase().includes(searchQuery.toLowerCase())
            );
            setFilteredData(filteredRequests);
        }
    }, [searchQuery, activeRequests]);

    const handleMenuClose = () => {
        setUpdate(true);
    };

    const handleAddButton = () => {
        navigate('/requests/add');
    };

    return (
        <div>
            <Typography variant="h3" style={{ justifyContent: 'center', marginBottom: '20px' }} >
                <Assignment sx={{ color: 'black', width: '75px', height: '75px' }} />
                { t(screenName + "title") }
            </Typography>
            <Button variant="contained" color='secondary' onClick={handleAddButton}>
                { t(screenName + "addButton") }
            </Button>
            <Divider sx={{ my: 2 }} />
            <>
                {isLoading ? (
                    <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
                ) : (
                    <>
                        {data.length != 0 ? (
                            <>
                                {activeRequests.length != 0 ? (
                                    < div >
                                        <h2>{ t(screenName + "activeRequests.title") }</h2>
                                        <TextField
                                            id='searchQuery'
                                            name='searchQuery'
                                            sx={{ mt: 2, width: '25%' }}
                                            label={ t(screenName + "activeRequests.query.label") }
                                            variant="outlined"
                                            value={searchQuery}
                                            onChange={(e) => setSearchQuery(e.target.value)}
                                        />
                                        <GridTable rows={filteredData as []} columnName={'requests'} onMenuClose={handleMenuClose} />
                                    </div>
                                ) : (
                                    <h2>{ t(screenName + "activeRequests.query.error") }</h2>
                                )}
                                <Divider sx={{ my: 2 }} />
                                {pastRequests.length != 0 ? (
                                    <div>
                                        <h2>{ t(screenName + "pastRequests.title") }</h2>
                                        <GridTable rows={pastRequests as []} columnName={'requests'} onMenuClose={handleMenuClose} />
                                    </div>
                                ) : (
                                    <h2>{ t(screenName + "pastRequests.error") }</h2>
                                )}
                            </>
                        ) : (
                            <>
                                {!error && <h2>{ t(screenName + "error") }</h2>}
                                {error && <h2>{error}</h2>}
                            </>
                        )}
                    </>
                )}
            </>
        </div>
    );
}

export default RequestsPage