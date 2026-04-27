import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { Package, Truck, CheckCircle, XCircle, Clock } from 'lucide-react'
import { api } from '@/lib/api'
import { Button } from '@/components/ui/Button'

interface OrderDto {
  id: string
  orderNumber: string
  customerName: string
  status: string
  totalAmount: number
  createdAt: string
}

export function OrdersPage() {
  const queryClient = useQueryClient()

  const { data, isLoading } = useQuery({
    queryKey: ['admin-orders'],
    queryFn: async () => {
      const { data } = await api.get('/orders?pageSize=50')
      return data
    }
  })

  const statusMutation = useMutation({
    mutationFn: ({ id, action }: { id: string, action: string }) => api.patch(`/orders/${id}/status`, { action }),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['admin-orders'] })
  })

  return (
    <div className="p-8 space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold tracking-tight text-slate-100">Orders Management</h1>
      </div>

      <div className="rounded-md border border-slate-800 bg-slate-900 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-sm text-left">
            <thead className="text-xs text-slate-400 uppercase bg-slate-950/50 border-b border-slate-800">
              <tr>
                <th className="px-6 py-4 font-medium">Order #</th>
                <th className="px-6 py-4 font-medium">Date</th>
                <th className="px-6 py-4 font-medium">Customer</th>
                <th className="px-6 py-4 font-medium">Total</th>
                <th className="px-6 py-4 font-medium">Status</th>
                <th className="px-6 py-4 font-medium text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {isLoading ? (
                <tr><td colSpan={6} className="px-6 py-8 text-center text-slate-400">Loading...</td></tr>
              ) : data?.items?.length === 0 ? (
                <tr><td colSpan={6} className="px-6 py-8 text-center text-slate-400">No orders found.</td></tr>
              ) : (
                data?.items?.map((o: OrderDto) => (
                  <tr key={o.id} className="border-b border-slate-800/50 hover:bg-slate-800/20 transition-colors">
                    <td className="px-6 py-4 font-medium text-blue-400">{o.orderNumber}</td>
                    <td className="px-6 py-4 text-slate-300">{new Date(o.createdAt).toLocaleDateString()}</td>
                    <td className="px-6 py-4 text-slate-200">{o.customerName}</td>
                    <td className="px-6 py-4 font-medium text-slate-100">${o.totalAmount.toFixed(2)}</td>
                    <td className="px-6 py-4">
                      <StatusBadge status={o.status} />
                    </td>
                    <td className="px-6 py-4 flex items-center justify-end gap-2">
                      {o.status === 'Pending' && (
                        <Button size="sm" variant="outline" onClick={() => statusMutation.mutate({ id: o.id, action: 'confirm' })}>
                          Confirm
                        </Button>
                      )}
                      {o.status === 'Confirmed' && (
                        <Button size="sm" variant="outline" onClick={() => statusMutation.mutate({ id: o.id, action: 'process' })}>
                          Process
                        </Button>
                      )}
                      {o.status === 'Processing' && (
                        <Button size="sm" onClick={() => statusMutation.mutate({ id: o.id, action: 'ship' })}>
                          <Truck className="h-4 w-4 mr-2" /> Ship
                        </Button>
                      )}
                      {o.status === 'Shipped' && (
                        <Button size="sm" className="bg-green-600 hover:bg-green-500 text-white" onClick={() => statusMutation.mutate({ id: o.id, action: 'deliver' })}>
                          <CheckCircle className="h-4 w-4 mr-2" /> Deliver
                        </Button>
                      )}
                      {(o.status === 'Pending' || o.status === 'Confirmed') && (
                        <Button size="sm" variant="ghost" className="text-red-400 hover:text-red-300 hover:bg-red-400/10" onClick={() => statusMutation.mutate({ id: o.id, action: 'cancel' })}>
                          <XCircle className="h-4 w-4" />
                        </Button>
                      )}
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  )
}

function StatusBadge({ status }: { status: string }) {
  const styles: Record<string, string> = {
    Pending: 'bg-yellow-500/10 text-yellow-400 border-yellow-500/20',
    Confirmed: 'bg-blue-500/10 text-blue-400 border-blue-500/20',
    Processing: 'bg-purple-500/10 text-purple-400 border-purple-500/20',
    Shipped: 'bg-indigo-500/10 text-indigo-400 border-indigo-500/20',
    Delivered: 'bg-green-500/10 text-green-400 border-green-500/20',
    Cancelled: 'bg-red-500/10 text-red-400 border-red-500/20',
  }

  const icons: Record<string, any> = {
    Pending: Clock,
    Confirmed: CheckCircle,
    Processing: Package,
    Shipped: Truck,
    Delivered: CheckCircle,
    Cancelled: XCircle,
  }

  const Icon = icons[status] || Clock
  const className = styles[status] || styles.Pending

  return (
    <span className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium border ${className}`}>
      <Icon className="h-3.5 w-3.5" />
      {status}
    </span>
  )
}
