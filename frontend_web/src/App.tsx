import './App.css'

import SideBar from '@/components/SideBar/SideBar';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LibrarianFrontPage from '@/pages/FrontPages/LibrarianFrontPage/LibrarianFrontPage';
import { PrivateRoute } from '@/utils/PrivateRoute'
import { AuthProvider } from '@/context/AuthContext';
import Status404 from '@/pages/Status/Status404';
import AddBookPage from '@/pages/Books/AddBookPage/AddBookPage';
import EditBookPage from '@/pages/Books/EditBookPage/EditBookPage';

import AuthorsPage from '@/pages/Authors/AuthorsPage';
import BooksPage from '@/pages/Books/BooksPage';
import ViewBookPage from '@/pages/Books/ViewBookPage';
import BookCopiesPage from './pages/Books/BookCopiesPage';
import CategoriesPage from '@/pages/Categories/CategoriesPage';
import LoginPage from '@/pages/Login/LoginPage';
import AddRequestPage from '@/pages/Requests/AddRequestPage';
import RequestsPage from '@/pages/Requests/RequestsPage';
import AddTransferPage from '@/pages/Transfers/AddTransferPage';
import TransfersPage from '@/pages/Transfers/TransfersPage';
import UsersPage from '@/pages/Users/UsersPage';
import UsersPunishmentsPage from '@/pages/Users/UsersPunishmentsPage';
import UsersRequestsPage from '@/pages/Users/UsersRequestsPage';
import NotificationsPage from '@/pages/Notifications/NotificationsPage';
import ProfilePage from './pages/Profile/ProfilePage';
import AdminFrontPage from './pages/FrontPages/AdminFrontPage/AdminFrontPage';
import LanguagesPage from './pages/Languages/LanguagesPage';

function App() {

  const routes = [
    {
      path: "/login",
      element: <LoginPage />,
      requiresAuth: false,
    },
    {
      path: "/",
      element: "",
      requiresAuth: true,
      roles: ["Librarian, Admin"],
    },
    {
      path: "/librarian",
      element: <LibrarianFrontPage />,
      requiresAuth: true,
      roles: ["Librarian"],
    },
    {
      path: "/admin",
      element: <AdminFrontPage />,
      requiresAuth: true,
      roles: ["Admin"],
    },
    {
      path: "/users",
      element: <UsersPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/users/punishments",
      element: <UsersPunishmentsPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/users/requests",
      element: <UsersRequestsPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/requests",
      element: <RequestsPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/requests/add",
      element: <AddRequestPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/transfers",
      element: <TransfersPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/transfers/add",
      element: <AddTransferPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/authors",
      element: <AuthorsPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/categories",
      element: <CategoriesPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/books",
      element: <BooksPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "books/view",
      element: <ViewBookPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "books/add",
      element: <AddBookPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "books/edit",
      element: <EditBookPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/books/physical-books",
      element: <BookCopiesPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/notifications",
      element: <NotificationsPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "*",
      element: <Status404 />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/profile",
      element: <ProfilePage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
    {
      path: "/adm",
      element: <AdminFrontPage />,
      requiresAuth: true,
      roles: ["Admin"],
    },
    {
      path: "/languages",
      element: <LanguagesPage />,
      requiresAuth: true,
      roles: ["Librarian", "Admin"],
    },
  ];


  return (
    <div className="App">
      <Router>
        <AuthProvider>
          <div className="left-column">
            <SideBar />
          </div>
          <div className="right-column">
            <div className="screen">
              <Routes>
                {routes.map((route) => {
                  return (
                    <Route
                      key={route.path}
                      path={route.path}
                      element={
                        route.requiresAuth ? (
                          <PrivateRoute path={route.path} roles={route.roles}>{route.element}</PrivateRoute>
                        ) : (
                          route.element
                        )
                      } />
                  );
                })}
              </Routes>
            </div>
          </div>
        </AuthProvider>
      </Router>
    </div >
  );
}

export default App