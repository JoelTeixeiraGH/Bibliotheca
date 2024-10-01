import { GridColDef } from '@mui/x-data-grid';
import { IconButton } from '@mui/material';
import { Settings, Delete } from '@mui/icons-material';
import { RequestStatus, getRequestStatusLabel } from '@/models/Request';
import { PhysicalBookStatus, getPhysicalBookStatusLabel } from '@/models/PhysicalBook';
import { TransferStatus, getTransferStatusLabel } from '@/models/Transfer';
import './GridTable.css';
import { useAuth } from '@/context/AuthContext';
import { useTranslation } from 'react-i18next';

interface RowType {
  defaultId: number;
  requestId: number;
  transferId: number;
  userId: number;
  punishmentId: number;
  authorId: number;
  categoryId: number;
  isbn: string;
  physicalBookId: number;
  notificationId: number;
  languageId: number;
  // Add other properties based on your actual data structure
}

export const getColumns = <T extends RowType>(columnName: string, onMenuOpen: (event: React.MouseEvent<HTMLButtonElement>, row: T) => void): GridColDef[] => {
  // eslint-disable-next-line react-hooks/rules-of-hooks
  const { t } = useTranslation();

  const { user } = useAuth();

  const screenName = "components.table.";

  switch (columnName) {
    case 'requests':
      return [
        { field: 'requestId', headerName: t(screenName + "requests.requestID"), type: 'number', flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'physicalId', headerName: t(screenName + "requests.physicalID"), type: 'number', flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.row.physicalBook?.physicalBookId || 'N/A', },
        { field: 'isbn', headerName: t(screenName + "requests.isbn"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'user', headerName: t(screenName + "requests.user"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.row.user?.userEmail || 'N/A', },
        { field: 'startDate', headerName: t(screenName + "requests.startDate"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'endDate', headerName: t(screenName + "requests.endDate"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.value ? params.value : 'N/A' },
        { field: 'requestStatus', headerName: t(screenName + "requests.requestStatus"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => getRequestStatusLabel(params.value as RequestStatus) },
        {
          field: 'actions', headerName: '', flex: 1, align: 'center', headerAlign: 'center', disableColumnMenu: true, sortable: false, headerClassName: 'header',
          renderCell: (params) => (
            <IconButton sx={{ color: 'black' }} onClick={(event) => onMenuOpen(event, params.row as T)}>
              <Settings />
            </IconButton>
          ),
        }
      ];
    case 'transfers':
      return [
        { field: 'transferId', headerName: t(screenName + "transfers.transferID"), type: 'number', flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'physicalBookId', headerName: t(screenName + "transfers.physicalBookID"), type: 'number', flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.row.physicalBook.physicalBookId, },
        { field: 'physicalBookStatus', headerName: t(screenName + "transfers.physicalBookStatus"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => getPhysicalBookStatusLabel(params.row.physicalBook.physicalBookStatus as PhysicalBookStatus) },
        { field: 'sourceLibrary', headerName: t(screenName + "transfers.sourceLibrary"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.row.sourceLibrary.libraryAlias },
        { field: 'destinationLibrary', headerName: t(screenName + "transfers.destinationLibrary"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.row.destinationLibrary.libraryAlias },
        { field: 'startDate', headerName: t(screenName + "transfers.startDate"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'endDate', headerName: t(screenName + "transfers.endDate"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'transferStatus', headerName: t(screenName + "transfers.transferStatus"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => getTransferStatusLabel(params.value as TransferStatus) },
        {
          field: 'actions', headerName: '', flex: 1, align: 'center', headerAlign: 'center', disableColumnMenu: true, sortable: false, headerClassName: 'header',
          renderCell: (params) => (
            <IconButton sx={{ color: 'black' }} onClick={(event) => onMenuOpen(event, params.row as T)}>
              <Settings />
            </IconButton>
          ),
        }
      ];
    case 'users':
      return [
        { field: 'userId', headerName: t(screenName + "users.userID"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'userName', headerName: t(screenName + "users.userName"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'userEmail', headerName: t(screenName + "users.userEmail"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'library', headerName: t(screenName + "users.library"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.row.library?.libraryAlias || 'N/A', },
        { field: 'numberOfRequests', headerName: t(screenName + "users.numberOfRequests"), type: 'number', flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'numberOfPunishments', headerName: t(screenName + "users.numberOfPunishments"), type: 'number', flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        {
          field: 'actions', headerName: '', flex: 1, align: 'center', headerAlign: 'center', disableColumnMenu: true, sortable: false, headerClassName: 'header',
          renderCell: (params) => (
            <IconButton sx={{ color: 'black' }} onClick={(event) => onMenuOpen(event, params.row as T)}>
              <Settings />
            </IconButton>
          ),
        }
      ]
    case 'punishments':
      return [
        { field: 'requestId', headerName: t(screenName + "punishments.requestID"), type: 'number', flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.row.request?.requestId || 'N/A', },
        { field: 'punishmentId', headerName: t(screenName + "punishments.punishmentID"), type: 'number', flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'punishmentReason', headerName: t(screenName + "punishments.punishmentReason"), flex: 3, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'punishmentLevel', headerName: t(screenName + "punishments.punishmentLevel"), type: 'number', flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'userName', headerName: t(screenName + "punishments.userName"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.row.request?.userName || 'N/A', },
        {
          field: 'actions', headerName: '', flex: 1, align: 'center', headerAlign: 'center', disableColumnMenu: true, sortable: false, headerClassName: 'header',
          renderCell: (params) => (
            <IconButton sx={{ color: 'red' }} onClick={(event) => onMenuOpen(event, params.row as T)}>
              <Delete />
            </IconButton>
          ),
        }
      ]
    case 'authors':
      return [
        { field: 'authorId', headerName: t(screenName + "authors.authorID"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'authorName', headerName: t(screenName + "authors.authorName"), flex: 10, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        {
          field: 'actions', headerName: '', flex: 1, align: 'center', headerAlign: 'center', disableColumnMenu: true, sortable: false, headerClassName: 'header',
          renderCell: (params) => (
            <IconButton sx={{ color: 'black' }} onClick={(event) => onMenuOpen(event, params.row as T)}>
              <Settings />
            </IconButton>
          ),
        }
      ]
    case 'categories':
      return [
        { field: 'categoryId', headerName: t(screenName + "categories.categoryID"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'categoryName', headerName: t(screenName + "categories.categoryName"), flex: 10, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        {
          field: 'actions', headerName: '', flex: 1, align: 'center', headerAlign: 'center', disableColumnMenu: true, sortable: false, headerClassName: 'header',
          renderCell: (params) => (
            <IconButton sx={{ color: 'black' }} onClick={(event) => onMenuOpen(event, params.row as T)}>
              <Settings />
            </IconButton>
          ),
        }
      ]
    case 'genericBooks':
      return [
        { field: 'isbn', headerName: t(screenName + "genericBooks.isbn"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'title', headerName: t(screenName + "genericBooks.title"), flex: 3, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'pageNumber', headerName: t(screenName + "genericBooks.pageNumber"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'datePublished', headerName: t(screenName + "genericBooks.datePublished"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'language', headerName: t(screenName + "genericBooks.language"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.row.language?.languageAlias || 'N/A', },
        { field: 'authors', headerName: t(screenName + "genericBooks.authors"), flex: 2, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => (params.row.authors && params.row.authors.length > 0) ? params.row.authors.map((author: any) => author.authorName).join(', ') : 'N/A', },
        { field: 'categories', headerName: t(screenName + "genericBooks.categories"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => (params.row.categories && params.row.categories.length > 0) ? params.row.categories.map((category: any) => category.categoryName).join(', ') : 'N/A', },
        {
          field: 'count', headerName: t(screenName + "genericBooks.copies"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header',
          valueGetter: (params) => {
            const library = (params.row.physicalBooks || []).find((library: any) => library.libraryId === Number(user.LibraryId));
            return library ? library.count : '0';
          },
        },
        {
          field: 'actions', headerName: '', flex: 1, align: 'center', headerAlign: 'center', disableColumnMenu: true, sortable: false, headerClassName: 'header',
          renderCell: (params) => (
            <IconButton sx={{ color: 'black' }} onClick={(event) => onMenuOpen(event, params.row as T)}>
              <Settings />
            </IconButton>
          ),
        }
      ]
    case 'physicalBooks':
      return [
        { field: 'physicalBookId', headerName: t(screenName + "physicalBooks.physicalBookID"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header', },
        { field: 'genericBook', headerName: t(screenName + "physicalBooks.genericBook"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.row.genericBook.isbn },
        { field: 'libraryAlias', headerName: t(screenName + "physicalBooks.libraryAlias"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => params.row.library.libraryAlias || 'N/A' },
        { field: 'physicalBookStatus', headerName: t(screenName + "physicalBooks.physicalBookStatus"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header', renderCell: (params) => getPhysicalBookStatusLabel(params.value as PhysicalBookStatus) },
        {
          field: 'actions', headerName: '', flex: 1, align: 'center', headerAlign: 'center', disableColumnMenu: true, sortable: false, headerClassName: 'header',
          renderCell: (params) => (
            <IconButton sx={{ color: 'black' }} onClick={(event) => onMenuOpen(event, params.row as T)}>
              <Settings />
            </IconButton>
          ),
        }
      ]
    case 'notifications':
      return [
        { field: 'notificationId', headerName: t(screenName + "notifications.notificationID"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'notificationTitle', headerName: t(screenName + "notifications.notificationTitle"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'notificationDescription', headerName: t(screenName + "notifications.notificationDescription"), flex: 5, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'emittedDate', headerName: t(screenName + "notifications.emittedDate"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'endDate', headerName: t(screenName + "notifications.endDate"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'forAll', headerName: t(screenName + "notifications.forAll"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        /*{
          field: 'actions', headerName: '', flex: 1, align: 'center', headerAlign: 'center', disableColumnMenu: true, sortable: false, headerClassName: 'header',
          renderCell: (params) => (
            <IconButton sx={{ color: 'black' }} onClick={(event) => onMenuOpen(event, params.row as T)}>
              <Settings />
            </IconButton>
          ),
        } */
      ]
    case 'languages':
      return [
        { field: 'languageId', headerName: t(screenName + "languages.languageId"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'languageName', headerName: t(screenName + "languages.languageName"), flex: 3, align: 'center', headerAlign: 'center', headerClassName: 'header' },
        { field: 'languageAlias', headerName: t(screenName + "languages.languageAlias"), flex: 1, align: 'center', headerAlign: 'center', headerClassName: 'header' },
      ]
    // Add more cases as needed

    default:
      return [];
  }
};

export const getRowId = (columnName: string): (row: RowType) => string => {
  switch (columnName) {
    case "requests":
      return (row: RowType) => row.requestId.toString();
    case "transfers":
      return (row: RowType) => row.transferId.toString();
    case "users":
      return (row: RowType) => row.userId.toString();
    case "punishments":
      return (row: RowType) => row.punishmentId.toString();
    case "authors":
      return (row: RowType) => row.authorId.toString();
    case "categories":
      return (row: RowType) => row.categoryId.toString();
    case "genericBooks":
      return (row: RowType) => row.isbn.toString();
    case "physicalBooks":
      return (row: RowType) => row.physicalBookId.toString();
    case "notifications":
      return (row: RowType) => row.notificationId.toString();
    case "languages":
      return (row: RowType) => row.languageId.toString();
    default:
      // Return a default function or handle the case where the columnName is not recognized
      return (row: RowType) => row.defaultId?.toString();
  }
};