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

interface InputDateDialogProps {
    isDialogOpen: boolean;
    onDialogClose: () => void;
    onConfirmButton: () => void;
    inputDate: (value: string) => void;
}

const InputDateDialog: React.FC<InputDateDialogProps> = ({ isDialogOpen, onDialogClose, onConfirmButton, inputDate }) => {
    const { t } = useTranslation();
    const [date, setDate] = useState('');

    const screenName = "components.dialogs.inputDate.";

    const onDateChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = event.target.value;
        setDate(newValue);
        inputDate(newValue);
    };

    return (
        <>
            {/* Input Date Dialog */}
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
                <DialogTitle style={{ textAlign: 'center', fontSize: 30 }}>
                    {t(screenName + "title")}
                </DialogTitle>
                <DialogContent>
                    <TextField
                        fullWidth
                        id="date"
                        label={t(screenName + "label")}
                        type="date"
                        value={date}
                        onChange={onDateChange}
                        InputLabelProps={{
                            shrink: true,
                        }}
                        sx={{ mt: 2 }}
                        variant="outlined" />
                </DialogContent>
                <DialogActions style={{ justifyContent: 'center' }}>
                    <Button onClick={() => { onDialogClose(); }} color="primary" variant="contained">
                        {t(screenName + "actions.1")}
                    </Button>
                    <Button onClick={() => { onConfirmButton(); }} disabled={date === ''} color="secondary" variant="contained">
                        {t(screenName + "actions.2")}
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
}

export default InputDateDialog