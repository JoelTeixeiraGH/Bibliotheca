import { useEffect, useState } from 'react';
import People from '@mui/icons-material/People';
import { Divider, TextField, Typography } from '@mui/material';
import GridTable from '@/components/table/GridTable';
import { User } from '@/models/User';
import userService from '@/services/userService';
import { useAuth } from '@/context/AuthContext';
import { useTranslation } from 'react-i18next';
import { set } from 'date-fns';

export const screenName = "pages.UsersPage.";

const UsersPage = () => {
    const { t } = useTranslation();
    const [data, setData] = useState<User[]>([]);
    const [filteredData, setFilteredData] = useState<User[]>([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [update, setUpdate] = useState(false);

    const { user } = useAuth();

    useEffect(() => {
        setIsLoading(true);

        const fetchData = async () => {
            try {
                const response = await userService.getAllUsersByLibraryId(user.LibraryId);

                if (response.status === 200) {
                    setData(response.data);
                    setFilteredData(response.data);
                } else {
                    setData([]);
                    setFilteredData([]);
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
        if (Array.isArray(filteredData)) {
            const filteredData = data.filter(user =>
                user.userId.toString().includes(searchQuery) ||
                user.userName.toLowerCase().includes(searchQuery.toLowerCase()) ||
                user.userEmail.toLowerCase().includes(searchQuery.toLowerCase())
            );
            setFilteredData(filteredData);
        }
    }, [searchQuery]);

    const handleMenuClose = () => {
        setUpdate(true);
    };

    return (
        <div>
            <Typography variant="h3" style={{ justifyContent: 'center', marginBottom: '20px' }}>
                <People sx={{ color: 'black', width: '75px', height: '75px' }} />
                {t("pages.LibrarianFrontPage.table.users")}
            </Typography>
            <Divider sx={{ my: 2 }} />
            {isLoading ? (
                <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
            ) : (
                <>
                    {data.length != 0 ? (
                        <>
                            <TextField
                                id='searchQuery'
                                name='searchQuery'
                                label={t(screenName + "query.label")}
                                variant="outlined"
                                value={searchQuery}
                                onChange={(e) => setSearchQuery(e.target.value)}
                                sx={{ width: '25%' }} />
                            <GridTable rows={filteredData as []} columnName='users' onMenuClose={handleMenuClose} />
                        </>
                    ) : (
                        <>
                            {!error && <h2>{t(screenName + "query.error")}</h2>}
                            {error && <h2>{error}</h2>}
                        </>
                    )}
                </>
            )}
        </div>
    );
}

export default UsersPage;