import GridTable from "@/components/table/GridTable";
import requestService from "@/services/requestService";
import { Assignment } from "@mui/icons-material";
import { Button, Divider, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useSearchParams } from "react-router-dom";
import { screenName as topScreen } from "./UsersPage";

const UsersRequestsPage = () => {
    const { t } = useTranslation();
    const [queryParameters] = useSearchParams();
    const [data, setData] = useState<Request[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [update, setUpdate] = useState(false);
    const [error, setError] = useState(null);

    const screenName = topScreen + "requestsPage."

    useEffect(() => {
        setIsLoading(true);

        const userId: number = parseInt(queryParameters.get("userId") || "0");
        const fetchData = async () => {
            try {
                const response = await requestService.getAllRequestsByUserId(userId);

                if (response.status === 200) {
                    setData(response.data);
                }

            } catch (error: any) {
                setError(error.message);
            }
        };

        setIsLoading(false);
        fetchData();
        setUpdate(false);
    }, [update]);

    const handleMenuClose = () => {
        setUpdate(true);
    }

    const handleBackClick = () => {
        window.history.back();
    }

    return (
        <div>
            <Typography variant="h3" style={{ justifyContent: 'center' }}>
                <Assignment sx={{ color: 'black', width: '75px', height: '75px' }} />
                { t(screenName + "title") } {queryParameters.get("userId")}
            </Typography>
            <Button variant="contained" onClick={handleBackClick}>
                { t(screenName + "backButton") }
            </Button>
            <Divider sx={{ my: 2 }} />
            {isLoading ? (
                <img src="src\assets\loading.svg" alt="Loading" style={{ width: '60px', height: '60px' }} />
            ) : (
                <>
                    {data.length != 0 ? (
                        <GridTable rows={data as []} columnName={'requests'} onMenuClose={handleMenuClose} />
                    ) : (
                        <>
                            {!error && <h2>{ t(screenName + "error") }</h2>}
                            {error && <h2>{error}</h2>}
                        </>
                    )}
                </>
            )}
        </div>
    );
}

export default UsersRequestsPage;