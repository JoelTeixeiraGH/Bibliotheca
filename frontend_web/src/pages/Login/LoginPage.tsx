import { useState } from 'react';
import {
    Avatar,
    Button,
    TextField,
    Typography,
    Container,
    Card,
    CardHeader,
    Divider,
    CardContent
} from '@mui/material';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import { useAuth } from '@/context/AuthContext';
import { useTranslation } from 'react-i18next';

const LoginPage = () => {
    const { t } = useTranslation();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { handleLogin } = useAuth();

    const screenName = "pages.LoginPage.";

    const handleLoginClick = () => {
        handleLogin(email, password);
    }

    return (
        <Container component="main" maxWidth="xs" style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
            <Card sx={{ mt: 2, width: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <CardHeader
                    avatar={<Avatar sx={{ backgroundColor: 'grey', color: 'black', fontWeight: 'bold' }} > <LockOutlinedIcon /> </Avatar>}
                    titleTypographyProps={{ variant: 'h6', fontWeight: 'bold' }}
                    title="Sign in" />
                <Divider />
                <CardContent sx={{ width: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                    <TextField
                        variant="outlined"
                        margin="normal"
                        required
                        fullWidth
                        id="email"
                        label={ t(screenName + "input.emailLabel") }
                        name="email"
                        autoComplete="email"
                        autoFocus
                        value={email}
                        error={!!error}
                        helperText={error}
                        onChange={(e) => {
                            setError('');
                            setEmail(e.target.value);
                        }}
                    />
                    <TextField
                        variant="outlined"
                        margin="normal"
                        required
                        fullWidth
                        name="password"
                        label={ t(screenName + "input.passwordLabel") }
                        type="password"
                        id="password"
                        value={password}
                        error={!!error}
                        helperText={error}
                        onChange={(e) => {
                            setError('');
                            setPassword(e.target.value);
                        }}
                    />
                    {error && (
                        <Typography color="error" variant="body2">
                            {error}
                        </Typography>
                    )}
                    <Divider sx={{ mt: 2 }} />
                    <Button type="submit" variant="contained" color="secondary" onClick={handleLoginClick} >
                        { t(screenName + "signinButton") }
                    </Button>
                </CardContent>
            </Card>
        </Container>
    );
};

export default LoginPage;
