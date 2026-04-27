import { Outlet, Navigate, Link, useLocation } from 'react-router-dom'
import { LayoutDashboard, PackageSearch, ShoppingBag, FileText, LogOut, Store } from 'lucide-react'
import { useAuthStore } from '@/store/auth.store'
import { cn } from '@/lib/utils'

export function AdminLayout() {
  const { role, logout, email } = useAuthStore()
  const location = useLocation()

  if (role !== 'Admin' && role !== 'Manager') {
    return <Navigate to="/login" replace />
  }

  const navigation =[
    { name: 'Dashboard', href: '/admin/dashboard', icon: LayoutDashboard },
    { name: 'Products & Stock', href: '/admin/products', icon: PackageSearch },
    { name: 'Orders', href: '/admin/orders', icon: ShoppingBag },
    { name: 'Reports', href: '/admin/reports', icon: FileText },
    { name: 'Storefront', href: '/catalog', icon: Store },
  ]

  return (
    <div className="min-h-screen bg-slate-950 flex">
      <div className="w-64 border-r border-slate-800 bg-slate-900 flex flex-col hidden md:flex">
        <div className="h-16 flex items-center px-6 border-b border-slate-800">
          <span className="text-lg font-bold text-blue-500">WMS Portal</span>
        </div>
        
        <nav className="flex-1 px-4 py-6 space-y-1">
          {navigation.map((item) => {
            const isActive = location.pathname.startsWith(item.href)
            return (
              <Link
                key={item.name}
                to={item.href}
                className={cn(
                  "flex items-center gap-3 px-3 py-2.5 rounded-md text-sm font-medium transition-colors",
                  isActive 
                    ? "bg-blue-600/10 text-blue-400" 
                    : "text-slate-400 hover:bg-slate-800 hover:text-slate-200"
                )}
              >
                <item.icon className="h-5 w-5" />
                {item.name}
              </Link>
            )
          })}
        </nav>

        <div className="p-4 border-t border-slate-800">
          <div className="flex items-center gap-3 px-3 py-2 text-sm text-slate-400">
            <div className="flex-1 truncate">{email}</div>
            <button onClick={logout} className="hover:text-red-400 p-1 rounded-md transition-colors" title="Logout">
              <LogOut className="h-5 w-5" />
            </button>
          </div>
        </div>
      </div>

      <div className="flex-1 flex flex-col min-w-0 overflow-hidden">
        <main className="flex-1 overflow-y-auto">
          <Outlet />
        </main>
      </div>
    </div>
  )
}
