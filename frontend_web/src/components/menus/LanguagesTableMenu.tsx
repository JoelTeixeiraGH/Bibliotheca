import { Edit, Delete } from "@mui/icons-material";
import { Divider, Menu, MenuItem } from "@mui/material";
import InputNameDialog from "@/components/dialogs/InputNameDialog";
import SuccessDialog from "@/components/dialogs/SuccessDialog";
import FailureDialog from "@/components/dialogs/FailureDialog";
import { useState } from "react";
import { Category } from "@/models/Category";
import { useTranslation } from "react-i18next";
import languageService from "@/services/languageService";

interface LanguagesTableMenuProps {
    selectedRow: Category;
    anchorEl: HTMLElement | null;
    onClose: () => void;
}

const LanguagesTableMenu: React.FC<LanguagesTableMenuProps> = ({ selectedRow, anchorEl, onClose }) => {
    const { t } = useTranslation();
    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);
    const [isInputNameDialogOpen, setInputNameDialogOpen] = useState(false);
    const [languageName, setLanguageName] = useState('');
    const [languageAlias, setLanguageAlias] = useState("");

    const screenName = "components.menus.languages.";
    var msg_failure = "components.dialogs.failure.";
    var msg_sucess = "components.dialogs.sucesss.";

    const handleEditClick = () => {
        setInputNameDialogOpen(true);
    };

    const handleDeleteClick = () => { };

    const handleNameChange = (value: string) => {
        setLanguageName(value);
        return value;
    }

    const handleAliasChange = (value: string) => {
        setLanguageAlias(value);
        return value;
    }

    const handleConfirm = async () => {
        handleDialogClose();

        const languageData = {
            languageId: selectedRow.languageId,
            languageName: languageName,
            languageAlias: languageAlias,
        }

        try {
            const response = await languageService.updateLanguage(languageData);

            if (response.status === 201) {
                setSuccessDialogOpen(true);
            } else {
                if (response.status === (400 | 500)) {
                    msg_failure = "components.dialogs.failure.error." + response.status; //não esquecer de mudar isto no i18n
                }
                setFailureDialogOpen(true);
            }

        } catch (error) {
            setFailureDialogOpen(true);
        }

        setLanguageName('');
        onClose();
    }

    const handleDialogClose = () => {
        setInputNameDialogOpen(false);
        setSuccessDialogOpen(false);
        setFailureDialogOpen(false);
    }

    return (
        <>
            <Menu
                id="rowMenu"
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={onClose}>
                <MenuItem key="edit" onClick={handleEditClick}>
                    <Edit sx={{ marginRight: 1 }} /> {t(screenName + "menu.1")}
                </MenuItem>
                <Divider />
                <MenuItem key="delete" onClick={handleDeleteClick}>
                    <Delete sx={{ marginRight: 1 }} /> {t(screenName + "menu.2")}
                </MenuItem>
            </Menu>

            <InputNameDialog
                title="Edit Language"
                isDialogOpen={isInputNameDialogOpen}
                onDialogClose={handleDialogClose}
                onConfirmButton={handleConfirm}
                inputName={handleNameChange}
                initialInputValue={selectedRow?.languageName} />

            {isSuccessDialogOpen && (
                <SuccessDialog
                    isDialogOpen={isSuccessDialogOpen}
                    onDialogClose={handleDialogClose}
                    msg={t(msg_sucess)}
                />
            )}

            {isFailureDialogOpen && (
                <FailureDialog
                    isDialogOpen={isFailureDialogOpen}
                    onDialogClose={handleDialogClose}
                    msg={t(msg_failure)}
                />
            )}
        </>
    );
}

export default LanguagesTableMenu;