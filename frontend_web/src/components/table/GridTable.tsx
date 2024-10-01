import {
    DataGrid,
    gridPageCountSelector,
    GridPagination,
    useGridApiContext,
    useGridSelector,
} from '@mui/x-data-grid';
import { TablePaginationProps } from '@mui/material/TablePagination';
import MuiPagination from '@mui/material/Pagination';
import React, { useState } from 'react';
import { getColumns, getRowId } from './GridTableColumns';
import RequestsTableMenu from '@/components/menus/RequestsTableMenu';
import TransfersTableMenu from '@/components/menus/TransfersTableMenu';
import UsersTableMenu from '@/components/menus/UsersTableMenu';
import punishmentService from '@/services/punishmentService';
import { Punishment } from '@/models/Punishment';
import { User } from '@/models/User';
import { Transfer } from '@/models/Transfer';
import SuccessDialog from '@/components/dialogs/SuccessDialog';
import FailureDialog from '@/components/dialogs/FailureDialog';
import AuthorsTableMenu from '@/components/menus/AuthorsTableMenu';
import CategoriesTableMenu from '@/components/menus/CategoriesTableMenu';
import { Category } from '@/models/Category';
import { Author } from '@/models/Author';
import { GenericBook } from '@/models/GenericBook';
import GenericBooksTableMenu from '@/components/menus/GenericBooksTableMenu';
import { useTranslation } from 'react-i18next';
import PhysicalBooksTableMenu from '../menus/PhysicalBooksTableMenu';
import LanguagesTableMenu from '../menus/LanguagesTableMenu';

interface GridTableProps {
    rows: [];
    columnName: string;
    onMenuClose: () => void;
}

type Handlers = {
    [key: string]: (event: React.MouseEvent<HTMLButtonElement>, row: any) => void;
};

const GridTable = ({ rows, columnName, onMenuClose }: GridTableProps) => {
    const [requestsMenuAnchor, setRequestsMenuAnchor] = useState<null | HTMLElement>(null);
    const [transfersMenuAnchor, setTransfersMenuAnchor] = useState<null | HTMLElement>(null);
    const [usersMenuAnchor, setUsersMenuAnchor] = useState<null | HTMLElement>(null);
    const [authorsMenuAnchor, setAuthorsMenuAnchor] = useState<null | HTMLElement>(null);
    const [categoriesMenuAnchor, setCategoriesMenuAnchor] = useState<null | HTMLElement>(null);
    const [genericBooksMenuAnchor, setGenericBooksMenuAnchor] = useState<null | HTMLElement>(null);
    const [physicalBooksMenuAnchor, setPhysicalBooksMenuAnchor] = useState<null | HTMLElement>(null);
    const [notificationsMenuAnchor, setNotificationsMenuAnchor] = useState<null | HTMLElement>(null);
    const [languagesMenuAnchor, setLanguagesMenuAnchor] = useState<null | HTMLElement>(null);
    const [selectedRow, setSelectedRow] = useState<any>();

    const [isSuccessDialogOpen, setSuccessDialogOpen] = useState(false);
    const [isFailureDialogOpen, setFailureDialogOpen] = useState(false);

    const [msg_success, setMsgSucess] = useState("");
    const [msg_failure, setMsgFailure] = useState("");

    const { t } = useTranslation();

    const handleRequestsMenuOpen = (event: React.MouseEvent<HTMLButtonElement>, row: Request) => {
        setRequestsMenuAnchor(event.currentTarget);
        setSelectedRow(row);
    };
    const handleTransfersMenuOpen = (event: React.MouseEvent<HTMLButtonElement>, row: Transfer) => {
        setTransfersMenuAnchor(event.currentTarget);
        setSelectedRow(row);
    };
    const handleUsersMenuOpen = (event: React.MouseEvent<HTMLButtonElement>, row: User) => {
        setUsersMenuAnchor(event.currentTarget);
        setSelectedRow(row);
    };
    const handleDeletePunishment = (event: React.MouseEvent<HTMLButtonElement>, row: Punishment) => {
        setSelectedRow(row);
        deletePunishment();
    };
    const handleOnAuthorsMenuOpen = (event: React.MouseEvent<HTMLButtonElement>, row: Author) => {
        setAuthorsMenuAnchor(event.currentTarget);
        setSelectedRow(row);
    };
    const handleOnCategoriesMenuOpen = (event: React.MouseEvent<HTMLButtonElement>, row: Category) => {
        setCategoriesMenuAnchor(event.currentTarget);
        setSelectedRow(row);
    };
    const handleOnGenericBooksMenuOpen = (event: React.MouseEvent<HTMLButtonElement>, row: GenericBook) => {
        setGenericBooksMenuAnchor(event.currentTarget);
        setSelectedRow(row);
    };
    const handleOnPhysicalBooksMenuOpen = (event: React.MouseEvent<HTMLButtonElement>, row: GenericBook) => {
        setPhysicalBooksMenuAnchor(event.currentTarget);
        setSelectedRow(row);
    };
    const handleOnNotificationsMenuOpen = (event: React.MouseEvent<HTMLButtonElement>, row: any) => {
        setNotificationsMenuAnchor(event.currentTarget);
        setSelectedRow(row);
    }
    const handleOnLanguagesMenuOpen = (event: React.MouseEvent<HTMLButtonElement>, row: any) => {
        setLanguagesMenuAnchor(event.currentTarget);
        setSelectedRow(row);
    }


    const deletePunishment = async () => {
        try {
            const response = await punishmentService.deletePunishment(selectedRow.punishmentId);

            if (response.status === 200) {
                setMsgSucess("components.dialogs.success.punishment.delete")
                setSuccessDialogOpen(true);
            } else {
                setMsgFailure("components.dialogs.failure.punishment.delete")
                setFailureDialogOpen(true);
            }

        } catch (error: any) {
            if (error.response.status === 500) {
                setMsgFailure("components.dialogs.failure.error_500")
                setFailureDialogOpen(true);
            }
        }

        onMenuClose();
    };

    const handleMenuClose = () => {
        setRequestsMenuAnchor(null);
        setTransfersMenuAnchor(null);
        setUsersMenuAnchor(null);
        setAuthorsMenuAnchor(null);
        setCategoriesMenuAnchor(null);
        setGenericBooksMenuAnchor(null);
        setPhysicalBooksMenuAnchor(null);
        setNotificationsMenuAnchor(null);
        setLanguagesMenuAnchor(null);
    };

    const handlers: Handlers = {
        requests: handleRequestsMenuOpen,
        transfers: handleTransfersMenuOpen,
        users: handleUsersMenuOpen,
        punishment: handleDeletePunishment,
        authors: handleOnAuthorsMenuOpen,
        categories: handleOnCategoriesMenuOpen,
        genericBooks: handleOnGenericBooksMenuOpen,
        physicalBooks: handleOnPhysicalBooksMenuOpen,
        notifications: handleOnNotificationsMenuOpen,
        languages: handleOnLanguagesMenuOpen
    };

    return (
        <>
            <DataGrid
                rows={rows}
                columns={getColumns(columnName, handlers[columnName])}
                initialState={{
                    pagination: { paginationModel: { pageSize: 10 } },
                }}
                pageSizeOptions={[10]}
                getRowId={getRowId(columnName)}
                sx={{
                    mt: 2,
                    mb: 2,
                    '& .MuiDataGrid-root': {
                        border: '1px solid #ccc', // Border for the entire grid
                    },
                    '& .MuiDataGrid-row': {
                        borderBottom: '1px solid #ddd', // Border between rows
                    },
                    '& .MuiDataGrid-columnHeader, & .MuiDataGrid-cell': {
                        borderRight: '1px solid #ddd', // Border between columns and cells
                    },
                    '& .MuiPaginationItem-root': {
                        borderRadius: '0', // Remove pagination item border-radius
                    },
                    '& .MuiDataGrid-colCellTitle, & .MuiDataGrid-footer, & .MuiDataGrid-footerContainer': {
                        backgroundColor: '#f0f0f0', // Grey background color for header and footer
                    },
                    '.css-levciy-MuiTablePagination-displayedRows': {
                        display: 'none', // Hide the "Rows per page" text
                    },
                }}
                pagination
                slots={{
                    pagination: CustomPagination,
                }} />
            {/* Action Button Menus */}
            {columnName === 'requests' && <RequestsTableMenu selectedRow={selectedRow} anchorEl={requestsMenuAnchor} onClose={() => { handleMenuClose(); onMenuClose(); }} />}
            {columnName === 'transfers' && <TransfersTableMenu selectedRow={selectedRow} anchorEl={transfersMenuAnchor} onClose={() => { handleMenuClose(); onMenuClose(); }} />}
            {columnName === 'users' && <UsersTableMenu selectedRow={selectedRow} anchorEl={usersMenuAnchor} onClose={() => { handleMenuClose(); onMenuClose(); }} />}
            {columnName === 'authors' && <AuthorsTableMenu selectedRow={selectedRow} anchorEl={authorsMenuAnchor} onClose={() => { handleMenuClose(); onMenuClose(); }} />}
            {columnName === 'categories' && <CategoriesTableMenu selectedRow={selectedRow} anchorEl={categoriesMenuAnchor} onClose={() => { handleMenuClose(); onMenuClose(); }} />}
            {columnName === 'genericBooks' && <GenericBooksTableMenu selectedRow={selectedRow} anchorEl={genericBooksMenuAnchor} onClose={() => { handleMenuClose(); onMenuClose(); }} />}
            {columnName === 'physicalBooks' && <PhysicalBooksTableMenu selectedRow={selectedRow} anchorEl={physicalBooksMenuAnchor} onClose={() => { handleMenuClose(); onMenuClose(); }} />}
            {columnName === 'languages' && <LanguagesTableMenu selectedRow={selectedRow} anchorEl={languagesMenuAnchor} onClose={() => { handleMenuClose(); onMenuClose(); }} />}

            {/* Render SuccessDialog only when isSuccessDialogOpen is true */}
            {isSuccessDialogOpen && (
                <SuccessDialog
                    isDialogOpen={isSuccessDialogOpen}
                    onDialogClose={onMenuClose}
                    msg={t(msg_success)}
                />
            )}

            {/* Render FailureDialog only when isFailureDialogOpen is true */}
            {isFailureDialogOpen && (
                <FailureDialog
                    isDialogOpen={isFailureDialogOpen}
                    onDialogClose={onMenuClose}
                    msg={t(msg_failure)}
                />
            )}
        </>
    )
}

function Pagination({ page, onPageChange, className }: Pick<TablePaginationProps, 'page' | 'onPageChange' | 'className'>) {
    const apiRef = useGridApiContext();
    const pageCount = useGridSelector(apiRef, gridPageCountSelector);

    return (
        <MuiPagination
            color="secondary"
            className={className}
            count={pageCount}
            page={page + 1}
            onChange={(event, newPage) => {
                onPageChange(event as any, newPage - 1);
            }} />
    );
}

function CustomPagination(props: any) {
    return <GridPagination ActionsComponent={Pagination} {...props} />;
}

export default GridTable