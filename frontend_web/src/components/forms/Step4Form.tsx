import { Button } from '@mui/material';
import React from 'react'
import { useTranslation } from 'react-i18next';

interface Step4FormProps {
    onBackButton: () => void;
    onSubmitButton: () => void;
    isSubmitable: boolean;
}

const Step4Form: React.FC<Step4FormProps> = ({ onBackButton, onSubmitButton, isSubmitable }) => {
    const { t } = useTranslation();

    const screenName = "components.forms.step4.";

    return (
        <>
            <Button onClick={onBackButton} sx={{ mt: 3, mr: 1 }} variant="contained" color='primary'>
                {t(screenName + "button1")}
            </Button>
            <Button onClick={onSubmitButton} disabled={!isSubmitable} sx={{ mt: 3 }} variant="contained" color='secondary'>
                {t(screenName + "button2")}
            </Button>
        </>
    )
}

export default Step4Form;
