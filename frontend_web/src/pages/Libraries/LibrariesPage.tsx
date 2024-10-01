import React, { useState } from 'react';
import { Button, Container, TextField } from '@mui/material';
import { Library } from '@/models/Library';
import { useTranslation } from 'react-i18next';
import libraryService from '@/services/libraryService';
import SuccessDialog from '@/components/dialogs/SuccessDialog';
import FailureDialog from '@/components/dialogs/FailureDialog';

const LibrariesPage = () => {
    const { t } = useTranslation();
    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState<Library[]>([]);
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [error, setError] = useState(null);
    const [msg_success, setMsgSucess] = useState("components.dialogs.success.library.create");
    const [msg_failure, setMsgFailure] = useState("components.dialogs.failure.library.create");
    const [libraryDialog, setLibraryDialog] = useState(false);
    const [libraryAlias, setLibraryAlias] = useState("");
    const [libraryName, setLibraryName] = useState("");
    const [libraryAddress, setLibraryAddress] = useState("");

    const screenName = "pages.LibraryPage.";

    const handleAddLibrary = async () => {
        setLibraryDialog(false);

        const libraryData = {
            libraryName: libraryName,
            libraryAlias: libraryAlias,
            libraryAddress: libraryAddress
        };

        try {
            const response = await libraryService.addLibrary(libraryData);

            if (response.status === 201) {
                setMsgSucess("components.dialogs.success.library.create");
                setSuccessDialogOpen(true);
            } else {
                setMsgSucess("components.dialogs.failure.library.create");
                setFailureDialogOpen(true);
            }
        } catch (error: any) {
            setMsgFailure("components.dialogs.failure.error_500");
            setFailureDialogOpen(true);
        }
    };

    const handleDialogClose = () => {
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
        setLibraryAddress("");
        setLibraryAlias("");
        setLibraryName("");
    };

    return (
        <>
            <Container>
                <TextField
                    label={t("Library Name")}
                    value={libraryName}
                    onChange={(e) => setLibraryName(e.target.value)}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label={t("Library Alias")}
                    value={libraryAlias}
                    onChange={(e) => setLibraryAlias(e.target.value)}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label={t("Library Address")}
                    value={libraryAddress}
                    onChange={(e) => setLibraryAddress(e.target.value)}
                    fullWidth
                    margin="normal"
                />

                <Button
                    variant="contained"
                    color="secondary"
                    onClick={handleAddLibrary}
                >
                    {t("Add Library")}
                </Button>
            </Container>

            {/* Render SuccessDialog only when isSuccessDialogOpen is true */}
            {isSuccessDialogOpen && (
                <SuccessDialog
                    isDialogOpen={isSuccessDialogOpen}
                    onDialogClose={handleDialogClose}
                    msg={t(msg_success)}
                />
            )}

            {/* Render FailureDialog only when isFailureDialogOpen is true */}
            {isFailureDialogOpen && (
                <FailureDialog
                    isDialogOpen={isFailureDialogOpen}
                    onDialogClose={handleDialogClose}
                    msg={t(msg_failure)}
                />
            )}
        </>
    );
};

export default LibrariesPage;
