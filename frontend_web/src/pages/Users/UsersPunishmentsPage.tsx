import { useEffect, useState } from "react";
import GridTable from "@/components/table/GridTable";
import { Punishment } from "@/models/Punishment";
import punishmentService from "@/services/punishmentService";
import { Gavel } from "@mui/icons-material";
import { Button, Divider, Typography } from "@mui/material";
import { useSearchParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { screenName as topScreen } from "./UsersPage";

const UsersPunishmentsPage = () => {
    const { t } = useTranslation();
    const [queryParameters] = useSearchParams();
    const [isLoading, setIsLoading] = useState(false);
    const [punishments, setPunishments] = useState<Punishment[]>([]);
    const [update, setUpdate] = useState(false);
    const [error, setError] = useState(null);

    const screenName = topScreen + "punishmentsPage."

    useEffect(() => {
        setIsLoading(true);

        const userId: number = parseInt(queryParameters.get("userId") || "0");
        const fetchData = async () => {
            try {
                const response = await punishmentService.getAllPunishmentsByUserId(userId);

                if (response.status === 200) {
                    setPunishments(response.data);
                    console.log(response.data);
                } 

            } catch (error: any) {
                setError(error.message);
                if (error.response && error.response.status === 404) {
                    // tratar erro
                } else {
                    // Outros erros
                    setError(error.message);
                }
            }
        };

        setIsLoading(false);
        fetchData();
        setUpdate(false);
    }, [update]);

    const handleMenuClose = () => {
        setUpdate(true);
    };

    const handleBackClick = () => {
        window.history.back();
    }

    return (
        <div>
            <Typography variant="h3" style={{ justifyContent: 'center' }} >
                <Gavel sx={{ color: 'black', width: '75px', height: '75px' }} />
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
                    {punishments.length != 0 ? (
                        <GridTable rows={punishments as []} columnName={"punishments"} onMenuClose={handleMenuClose} />
                    ) : (
                        <>
                            {!error && <h2>{ t(screenName + "error") }</h2>}
                        </>
                    )}
                </>
            )}
        </div >
    );
}

export default UsersPunishmentsPage;