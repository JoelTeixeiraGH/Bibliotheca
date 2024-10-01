import { useEffect } from 'react';
import {
  Box,
  Card,
  Typography,
  Container,
  Button,
} from '@mui/material';

import { styled } from '@mui/material/styles';

const MainContent = styled(Box)(
  `
    height: 100%;
    display: flex;
    flex: 1;
    overflow: auto;
    flex-direction: column;
    align-items: center;
    justify-content: center;
  `
);

function Status404() {
  useEffect(() => {
    // Altera o título da página quando o componente é montado
    document.title = 'Error - 404';

    // Retorna uma função de limpeza para reverter as alterações no título quando o componente é desmontado
    return () => {
      document.title = 'LDS FrontEnd Web';
    };
  }, []);

  return (
    <>
      <MainContent>
        <Container maxWidth="md">
          <Box textAlign="center">
            <img alt="404" height={180} src="/static/images/status/404.svg" />
            <Typography variant="h2" sx={{ my: 2 }}>
              The page you were looking for doesn't exist.
            </Typography>
            <Typography
              variant="h4"
              color="text.secondary"
              fontWeight="normal"
              sx={{ mb: 4 }}>
              It's on us, we moved the content to a different page.
            </Typography>
          </Box>
          <Container maxWidth="sm">
            <Card sx={{ textAlign: 'center', mt: 3, p: 4 }}>
              <Button href="/" variant="outlined">
                Go to homepage
              </Button>
            </Card>
          </Container>
        </Container>
      </MainContent>
    </>
  );
}

export default Status404;
