import { createBrowserRouter, Navigate } from 'react-router-dom'
import { LoginPage } from '@/pages/auth/LoginPage'
import { RegisterPage } from '@/pages/auth/RegisterPage'
import { AdminLayout } from '@/layouts/AdminLayout'
import { DashboardPage } from '@/pages/admin/DashboardPage'
import { ProductsPage } from '@/pages/admin/ProductsPage'
import { ReportsPage } from '@/pages/admin/ReportsPage'
import { OrdersPage } from '@/pages/admin/OrdersPage'

import { CustomerLayout } from '@/layouts/CustomerLayout'
import { CatalogPage } from '@/pages/customer/CatalogPage'
import { CartPage } from '@/pages/customer/CartPage'
import { MyOrdersPage } from '@/pages/customer/MyOrdersPage'
import { NotFoundPage } from '@/pages/shared/NotFoundPage'

export const router = createBrowserRouter([
    {
      path: '/',
      element: <Navigate to="/login" replace />
    },
    {
      path: '/login',
      element: <LoginPage />
    },
    {
      path: '/register',
      element: <RegisterPage />
    },
    {
      path: '/admin',
      element: <AdminLayout />,
      children:[
        { path: 'dashboard', element: <DashboardPage /> },
        { path: 'products', element: <ProductsPage /> },
        { path: 'orders', element: <OrdersPage /> },
        { path: 'reports', element: <ReportsPage /> },
        { path: '*', element: <NotFoundPage /> }
      ]
    },
    {
      path: '/',
      element: <CustomerLayout />,
      children:[
        { path: 'catalog', element: <CatalogPage /> },
        { path: 'cart', element: <CartPage /> },
        { path: 'my-orders', element: <MyOrdersPage /> },
        { path: '*', element: <NotFoundPage /> }
      ]
    },
    {
      path: '*',
      element: <NotFoundPage />
    }
  ])