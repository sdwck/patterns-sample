import { Outlet, Navigate, Link, useLocation } from 'react-router-dom'
import { Store, ShoppingCart, History, LogOut, Package, Shield } from 'lucide-react'
import { useAuthStore } from '@/store/auth.store'
import { useCartStore } from '@/store/cart.store'
import { cn } from '@/lib/utils'

export function CustomerLayout() {
  const { role, logout, email } = useAuthStore()
  const itemCount = useCartStore(state => state.getItemCount())
  const location = useLocation()

  if (!role) {
    return <Navigate to="/login" replace />
  }

  const navigation =[
    { name: 'Catalog', href: '/catalog', icon: Store },
    { name: 'My Orders', href: '/my-orders', icon: History },
  ]

  return (
    <div className="min-h-screen bg-slate-950 flex flex-col">
      <header className="border-b border-slate-800 bg-slate-900 sticky top-0 z-10">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between h-16">
            <div className="flex items-center gap-2">
              <Package className="h-8 w-8 text-blue-500" />
              <span className="text-xl font-bold text-slate-100">WMS Store</span>
            </div>
            
            <nav className="flex space-x-4">
              {navigation.map((item) => {
                const isActive = location.pathname.startsWith(item.href)
                return (
                  <Link
                    key={item.name}
                    to={item.href}
                    className={cn(
                      "flex items-center gap-2 px-3 py-2 rounded-md text-sm font-medium transition-colors",
                      isActive 
                        ? "bg-slate-800 text-blue-400" 
                        : "text-slate-400 hover:text-slate-200 hover:bg-slate-800/50"
                    )}
                  >
                    <item.icon className="h-4 w-4" />
                    {item.name}
                  </Link>
                )
              })}
            </nav>

            <div className="flex items-center gap-4">
              {(role === 'Admin' || role === 'Manager') && (
                <Link 
                  to="/admin/dashboard" 
                  className="flex items-center gap-2 px-3 py-1.5 rounded bg-blue-600/10 text-blue-400 hover:bg-blue-600/20 text-sm font-medium transition-colors"
                >
                  <Shield className="h-4 w-4" />
                  Admin Panel
                </Link>
              )}

              <Link 
                to="/cart" 
                className="relative p-2 text-slate-400 hover:text-blue-400 transition-colors"
              >
                <ShoppingCart className="h-6 w-6" />
                {itemCount > 0 && (
                  <span className="absolute top-0 right-0 inline-flex items-center justify-center px-2 py-1 text-xs font-bold leading-none text-white transform translate-x-1/4 -translate-y-1/4 bg-blue-600 rounded-full">
                    {itemCount}
                  </span>
                )}
              </Link>
              <div className="h-6 w-px bg-slate-800"></div>
              <span className="text-sm text-slate-400 hidden sm:block">{email}</span>
              <button 
                onClick={logout} 
                className="text-slate-400 hover:text-red-400 transition-colors p-2"
                title="Logout"
              >
                <LogOut className="h-5 w-5" />
              </button>
            </div>
          </div>
        </div>
      </header>

      <main className="flex-1 max-w-7xl w-full mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <Outlet />
      </main>
    </div>
  )
}
