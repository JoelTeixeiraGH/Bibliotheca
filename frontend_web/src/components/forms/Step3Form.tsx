import { TextField } from '@mui/material';
import React, { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next';

interface Step3FormProps {
    showEndDate: boolean
    onEndDate: (value: string) => void;
}

const Step3Form: React.FC<Step3FormProps> = ({ showEndDate, onEndDate }) => {
    const { t } = useTranslation();
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');

    const screenName = "components.forms.step3.";

    useEffect(() => {
        // Set the initial value to today's date
        const today = new Date();
        const formattedDate = today.toISOString().split('T')[0];
        setStartDate(formattedDate);

    }, [])

    const handleEndDateChange = (event: React.ChangeEvent<{ value: string }>) => {
        setEndDate(event.target.value);
        onEndDate(event.target.value);
    };

    return (
        <>
            <TextField
                fullWidth
                id="start-date"
                label={t(screenName + "label1")}
                type="date"
                value={startDate}
                InputLabelProps={{
                    shrink: true,
                }}
                InputProps={{
                    readOnly: true,
                }} />
            {showEndDate && (
                <TextField
                    fullWidth
                    id="end-date"
                    label={t(screenName + "label2")}
                    type="date"
                    value={endDate}
                    onChange={handleEndDateChange}
                    InputLabelProps={{
                        shrink: true,
                    }}
                    sx={{ mt: 2 }} />
            )}
        </>
    );
}

export default Step3Form