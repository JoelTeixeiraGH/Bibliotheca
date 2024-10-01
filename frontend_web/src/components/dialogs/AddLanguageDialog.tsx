import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
} from '@mui/material';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

interface AddLanguageDialogProps {
    title: string;
    isDialogOpen: boolean;
    onDialogClose: () => void;
    onConfirmButton: () => void;
    languageNameChange: (value: string) => void;
    languageAliasChange: (value: string) => void;
}

const AddLanguageDialog: React.FC<AddLanguageDialogProps> = ({
    isDialogOpen,
    onDialogClose,
    onConfirmButton,
    languageNameChange: languageNameChange,
    languageAliasChange: languageAliasChange
}) => {
    const { t } = useTranslation();
    const [languageName, setLanguageName] = useState('');
    const [languageAlias, setLanguageAlias] = useState("");

    const screenName = "components.dialogs.addLanguage.";

    const onLanguageNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = event.target.value;
        setLanguageName(newValue);
        languageNameChange(newValue);
    };

    const onLanguageAliasChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = event.target.value;
        setLanguageAlias(newValue);
        languageAliasChange(newValue);
    };

    const resetParams = () => {
        setLanguageName("");
        setLanguageAlias("");
    };

    return (
        <>
            <Dialog
                open={isDialogOpen}
                onClose={onDialogClose}
                maxWidth="xs"
                fullWidth
                PaperProps={{
                    sx: {
                        borderRadius: '10px',
                    },
                }} >
                <DialogTitle style={{ textAlign: 'center', fontSize: 30 }}>{screenName + "title"}</DialogTitle>
                <DialogContent>
                    <TextField
                        fullWidth
                        id="name"
                        label={t(screenName + "languageName")}
                        type="text"
                        value={languageName}
                        onChange={onLanguageNameChange}
                        sx={{ mt: 2 }} />
                </DialogContent>
                <DialogContent>
                    <TextField
                        fullWidth
                        id="alias"
                        label={t(screenName + "languageAlias")}
                        type="text"
                        value={languageAlias}
                        onChange={onLanguageAliasChange}
                        sx={{ mt: 2 }} />
                </DialogContent>
                <DialogActions style={{ justifyContent: 'center' }}>
                    <Button onClick={() => { onDialogClose(); resetParams(); }} color="primary" variant="contained">
                        {t(screenName + "actions.1")}
                    </Button>
                    <Button onClick={() => { onConfirmButton(); resetParams(); }}
                        disabled={languageAlias === ''} color="secondary" variant="contained">
                        {t(screenName + "actions.2")}
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
}

export default AddLanguageDialog;