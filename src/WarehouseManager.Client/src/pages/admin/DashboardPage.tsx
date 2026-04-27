import { useQuery } from '@tanstack/react-query'
import { Activity, Users, ShoppingCart, AlertTriangle } from 'lucide-react'
import { api } from '@/lib/api'

interface DashboardStats {
  totalOrders: number
  totalRevenue: number
  lowStockItemsCount: number
  totalCustomers: number
}

export function DashboardPage() {
  const { data: stats, isLoading, isError } = useQuery({
    queryKey:['dashboard-stats'],
    queryFn: async () => {
      const { data } = await api.get<DashboardStats>('/dashboard/stats')
      return data
    }
  })

  if (isLoading) return <div className="p-8 text-slate-400 animate-pulse">Loading dashboard...</div>
  if (isError) return <div className="p-8 text-red-400">Failed to load statistics.</div>

  const statCards =[
    { title: 'Total Revenue', value: `$${stats?.totalRevenue.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`, icon: Activity, color: 'text-green-500', bg: 'bg-green-500/10' },
    { title: 'Total Orders', value: stats?.totalOrders, icon: ShoppingCart, color: 'text-blue-500', bg: 'bg-blue-500/10' },
    { title: 'Total Customers', value: stats?.totalCustomers, icon: Users, color: 'text-purple-500', bg: 'bg-purple-500/10' },
    { title: 'Low Stock Alerts', value: stats?.lowStockItemsCount, icon: AlertTriangle, color: 'text-orange-500', bg: 'bg-orange-500/10' },
  ]

  return (
    <div className="p-8 space-y-6">
      <h1 className="text-3xl font-bold tracking-tight text-slate-100">Dashboard</h1>
      
      <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
        {statCards.map((item) => {
          const Icon = item.icon
          return (
            <div key={item.title} className="rounded-xl border border-slate-800 bg-slate-900 p-6 shadow-sm">
              <div className="flex items-center gap-4">
                <div className={`rounded-lg p-3 ${item.bg}`}>
                  <Icon className={`h-6 w-6 ${item.color}`} />
                </div>
                <div>
                  <p className="text-sm font-medium text-slate-400">{item.title}</p>
                  <p className="text-2xl font-bold text-slate-100">{item.value}</p>
                </div>
              </div>
            </div>
          )
        })}
      </div>
    </div>
  )
}
