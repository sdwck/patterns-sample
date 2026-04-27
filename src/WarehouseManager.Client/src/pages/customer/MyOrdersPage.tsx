import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { Package, RefreshCw } from 'lucide-react'
import { api } from '@/lib/api'
import { Button } from '@/components/ui/Button'

interface OrderItemDto {
  id: string
  productName: string
  quantity: number
  unitPrice: number
  total: number
}

interface OrderDto {
  id: string
  orderNumber: string
  status: string
  totalAmount: number
  createdAt: string
  items: OrderItemDto[]
}

export function MyOrdersPage() {
  const queryClient = useQueryClient()

  const { data, isLoading } = useQuery({
    queryKey: ['my-orders'],
    queryFn: async () => {
      const { data } = await api.get('/orders')
      return data
    }
  })

  const reorderMutation = useMutation({
    mutationFn: (orderId: string) => api.post(`/orders/${orderId}/reorder`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['my-orders'] })
  })

  if (isLoading) return <div className="text-slate-400 animate-pulse">Loading orders...</div>

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold tracking-tight text-slate-100">My Orders</h1>
        <p className="mt-1 text-slate-400">View your order history and reorder past purchases.</p>
      </div>

      <div className="space-y-6">
        {data?.items?.length === 0 ? (
          <div className="rounded-xl border border-slate-800 bg-slate-900 p-12 text-center">
            <Package className="mx-auto h-12 w-12 text-slate-600 mb-4" />
            <h3 className="text-lg font-medium text-slate-300">No orders yet</h3>
            <p className="text-slate-500">When you place an order, it will appear here.</p>
          </div>
        ) : (
          data?.items?.map((order: OrderDto) => (
            <div key={order.id} className="rounded-xl border border-slate-800 bg-slate-900 overflow-hidden">
              <div className="bg-slate-800/50 px-6 py-4 border-b border-slate-800 flex flex-col sm:flex-row sm:items-center justify-between gap-4">
                <div className="flex gap-6 text-sm">
                  <div>
                    <span className="block text-slate-500">Order Placed</span>
                    <span className="font-medium text-slate-200">{new Date(order.createdAt).toLocaleDateString()}</span>
                  </div>
                  <div>
                    <span className="block text-slate-500">Total</span>
                    <span className="font-medium text-slate-200">${order.totalAmount.toFixed(2)}</span>
                  </div>
                  <div>
                    <span className="block text-slate-500">Order #</span>
                    <span className="font-medium text-slate-200">{order.orderNumber}</span>
                  </div>
                </div>
                <div className="flex items-center gap-4">
                  <span className={`px-3 py-1 rounded-full text-xs font-medium border ${
                    order.status === 'Delivered' ? 'bg-green-500/10 text-green-400 border-green-500/20' :
                    order.status === 'Cancelled' ? 'bg-red-500/10 text-red-400 border-red-500/20' :
                    'bg-blue-500/10 text-blue-400 border-blue-500/20'
                  }`}>
                    {order.status}
                  </span>
                  <Button 
                    variant="outline" 
                    size="sm" 
                    className="gap-2"
                    onClick={() => reorderMutation.mutate(order.id)}
                    disabled={reorderMutation.isPending}
                  >
                    <RefreshCw className={`h-4 w-4 ${reorderMutation.isPending ? 'animate-spin' : ''}`} />
                    Reorder
                  </Button>
                </div>
              </div>
              <div className="p-6">
                <ul className="divide-y divide-slate-800">
                  {order.items.map(item => (
                    <li key={item.id} className="py-4 first:pt-0 last:pb-0 flex items-center justify-between">
                      <div className="flex items-center gap-4">
                        <div className="h-12 w-12 rounded bg-slate-800 flex items-center justify-center">
                          <Package className="h-6 w-6 text-slate-500" />
                        </div>
                        <div>
                          <p className="font-medium text-slate-200">{item.productName}</p>
                          <p className="text-sm text-slate-500">Qty: {item.quantity}</p>
                        </div>
                      </div>
                      <p className="font-medium text-slate-300">${item.total.toFixed(2)}</p>
                    </li>
                  ))}
                </ul>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  )
}
