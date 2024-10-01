import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
} from '@mui/material';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

interface InputNameDialogProps {
    title: string;
    isDialogOpen: boolean;
    onDialogClose: () => void;
    onConfirmButton: () => void;
    inputName: (value: string) => void;
    initialInputValue: string;
}

const InputNameDialog: React.FC<InputNameDialogProps> = ({ title, isDialogOpen, onDialogClose, onConfirmButton, inputName, initialInputValue }) => {
    const { t } = useTranslation();
    const [name, setName] = useState('');

    const screenName = "components.dialogs.inputName.";

    useEffect(() => {
        setName(initialInputValue);
    }, [initialInputValue]);

    const onNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = event.target.value;
        setName(newValue);
        inputName(newValue);
    };

    const resetName = () => {
        setName('');
    };

    return (
        <>
            {/* Input Name Dialog */}
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
                <DialogTitle style={{ textAlign: 'center', fontSize: 30 }}>{title}</DialogTitle>
                <DialogContent>
                    <TextField
                        fullWidth
                        id="Name"
                        label={t(screenName + "label")}
                        type="text"
                        value={name}
                        onChange={onNameChange}
                        sx={{ mt: 2 }} />
                </DialogContent>
                <DialogActions style={{ justifyContent: 'center' }}>
                    <Button onClick={() => { onDialogClose(); resetName(); }} color="primary" variant="contained">
                        {t(screenName + "actions.1")}
                    </Button>
                    <Button onClick={() => { onConfirmButton(); resetName(); }} disabled={name === ''} color="secondary" variant="contained">
                        {t(screenName + "actions.2")}
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
}

export default InputNameDialog