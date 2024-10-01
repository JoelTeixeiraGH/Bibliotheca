import { Button, Divider, TextField, Typography } from '@mui/material';
import TranslateIcon from "@mui/icons-material/Translate";
import GridTable from '@/components/table/GridTable';
import { useEffect, useState } from 'react';
import { Language } from '@/models/Language';
import languageService from '@/services/languageService';
import FailureDialog from '@/components/dialogs/FailureDialog';
import SuccessDialog from '@/components/dialogs/SuccessDialog';
import { useTranslation } from 'react-i18next';
import AddLanguageDialog from '@/components/dialogs/AddLanguageDialog';

const LanguagesPage = () => {
    const { t } = useTranslation();
    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState<Language[]>([]);
    const [filteredData, setfilteredData] = useState<Language[]>([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [update, setUpdate] = useState(false);
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [isInputNameDialogOpen, setInputNameDialogOpen] = useState(false);
    const [languageName, setLanguageName] = useState('');
    const [languageAlias, setLanguageAlias] = useState("");
    const [error, setError] = useState(null);
    const [msg_success, setMsgSucess] = useState("components.dialogs.success.language");
    const [msg_failure, setMsgFailure] = useState("components.dialogs.failure.language.400");

    const screenName = "pages.LanguagesPage.";


    useEffect(() => {
        setIsLoading(true);

        const fetchData = async () => {
            try {
                const response = await languageService.getAllLanguages();

                if (response.status === 200) {
                    setData(response.data);
                    setfilteredData(response.data);
                }

            } catch (error: any) {
                setError(error.message);
            }
        };

        fetchData();
        setUpdate(false);
        setIsLoading(false);
    }, [update]);

    useEffect(() => {
        // Filter data based on the search query
        if (Array.isArray(data)) {
            const filteredLanguages = data.filter(language =>
                language.languageId.toString().includes(searchQuery) ||
                language.languageName.toLowerCase().includes(searchQuery.toLowerCase()) ||
                language.languageAlias.toLowerCase().includes(searchQuery.toLowerCase())
            );
            setfilteredData(filteredLanguages);
        }
    }, []);

    const handleAddClick = () => {
        setInputNameDialogOpen(true);
    };

    const handleNameChange = (value: string) => {
        setLanguageName(value);
        return value;
    }

    const handleAliasChange = (value: string) => {
        setLanguageAlias(value);
        return value;
    }

    const handleConfirm = async () => {
        setInputNameDialogOpen(false);

        const languageData = {
          languageName: languageName,
          languageAlias: languageAlias,
        };

        try {
            const response = await languageService.createLanguage(languageData);
            
            if (response.status === 201) {
                setSuccessDialogOpen(true);
            } else if (response.status !== 200) {
                setMsgFailure(t(`components.dialogs.failure.language.${response.status}`));
                setFailureDialogOpen(true);
            } else {
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setFailureDialogOpen(true);
        }
    };

    const handleMenuClose = () => {
        setUpdate(true);
    };

    const handleDialogClose = () => {
        setLanguageName('');
        setInputNameDialogOpen(false);
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
        setUpdate(true);
    }

    return (
      <div>
        <Typography
          variant="h3"
          style={{ justifyContent: "center", marginBottom: "20px" }}
        >
          <TranslateIcon
            sx={{ color: "black", width: "75px", height: "75px" }}
          />
          {t(screenName + "title")}
        </Typography>
        <Button variant="contained" color="secondary" onClick={handleAddClick}>
          {t(screenName + "addButton")}
        </Button>
        <Divider sx={{ my: 2 }} />
        {isLoading ? (
          <img
            src="src\assets\loading.svg"
            alt="Loading"
            style={{ width: "60px", height: "60px" }}
          />
        ) : (
          <>
            {data.length != 0 ? (
              <>
                <TextField
                  id="searchQuery"
                  name="searchQuery"
                  sx={{ mt: 2, width: "25%" }}
                  label={t(screenName + "query.label")}
                  variant="outlined"
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                />
                <GridTable
                  rows={filteredData as []}
                  columnName={"languages"}
                  onMenuClose={handleMenuClose}
                />
              </>
            ) : (
              <>
                {!error && <h2>{t(screenName + "query.error")}</h2>}
                {error && <h2>{error}</h2>}
              </>
            )}
          </>
        )}
        <AddLanguageDialog
          title={t(screenName + "addDialogTitle")}
          isDialogOpen={isInputNameDialogOpen}
          onDialogClose={handleDialogClose}
          onConfirmButton={handleConfirm}
          languageNameChange={handleNameChange}
          languageAliasChange={handleAliasChange}
        />
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
      </div>
    );
}

export default LanguagesPage;