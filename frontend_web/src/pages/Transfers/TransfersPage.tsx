import { SwapHoriz } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { Button, Divider, TextField, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import transferService from '@/services/transferService';
import { Transfer, TransferStatus } from '@/models/Transfer';
import GridTable from '@/components/table/GridTable';
import { useAuth } from '@/context/AuthContext';
import { useTranslation } from 'react-i18next';

export const screenName = "pages.TransfersPage."

const TransfersPage = () => {
    const { t } = useTranslation();
    const [data, setData] = useState<Transfer[]>([]);
    const [filteredData, setFilteredData] = useState<Transfer[]>([]);
    const [activeTransfers, setActiveTransfers] = useState<Transfer[]>([]);
    const [pastTransfers, setPastTransfers] = useState<Transfer[]>([]);
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
                const response = await transferService.getAllTransfersByLibraryId(user.LibraryId);

                if (response.status === 200) {
                    setData(response.data);
                    setActiveTransfers(response.data.filter((transfer: Transfer) => transfer.transferStatus === TransferStatus.Pending));
                    setPastTransfers(response.data.filter((transfer: Transfer) => transfer.transferStatus != TransferStatus.Pending));
                    setFilteredData(activeTransfers);
                }

            } catch (error: any) {
                setError(error.message);
            }
        };

        setIsLoading(false);
        fetchData();
        setUpdate(false);
    }, [update]);

    useEffect(() => {
        // Filter data based on the search query
        if (Array.isArray(filteredData)) {
            const filteredTransfers = activeTransfers.filter(transfer => transfer.transferId.toString().includes(searchQuery));
            setFilteredData(filteredTransfers);
        }
    }, [searchQuery, activeTransfers]);

    const handleMenuClose = () => {
        setUpdate(true);
    };

    const handleAddButton = () => {
        navigate('/transfers/add');
    }

    return (
        <div>
            <Typography variant="h3" style={{ justifyContent: 'center', marginBottom: '20px' }} >
                <SwapHoriz sx={{ color: 'black', width: '75px', height: '75px' }} />
                {t(screenName + "title")}
            </Typography>
            <Button variant="contained" color='secondary' onClick={handleAddButton}>
                {t(screenName + "addButton")}
            </Button>
            <Divider sx={{ my: 2 }} />
            {isLoading ? (
                <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
            ) : (
                <>
                    {data.length != 0 ? (
                        <>
                            {activeTransfers.length != 0 ? (
                                < div >
                                    <h2>{t(screenName + "activeTransfers.title")}</h2>
                                    <TextField
                                        id='searchQuery'
                                        name='searchQuery'
                                        sx={{ mt: 2, width: '25%' }}
                                        label="Search for transfer (ID)"
                                        variant="outlined"
                                        value={searchQuery}
                                        onChange={(e) => setSearchQuery(e.target.value)}
                                    />
                                    <GridTable rows={filteredData as []} columnName={'transfers'} onMenuClose={handleMenuClose} />
                                </div>
                            ) : (
                                <h2>{t(screenName + "activeTransfers.error")}</h2>
                            )}
                            <Divider sx={{ my: 2 }} />
                            {pastTransfers.length != 0 ? (
                                <div>
                                    <h2>{t(screenName + "pastTransfers.title")}</h2>
                                    <GridTable rows={pastTransfers as []} columnName={'transfers'} onMenuClose={handleMenuClose} />
                                </div>
                            ) : (
                                <h2>{t(screenName + "pastTransfers.error")}</h2>
                            )}
                        </>
                    ) : (
                        <>
                            {!error && <h2>{t(screenName + "error")}</h2>}
                            {error && <h2>{error}</h2>}
                        </>
                    )}
                </>
            )}
        </div>
    );
}

export default TransfersPage