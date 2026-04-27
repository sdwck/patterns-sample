import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Trash2, ShoppingBag, ArrowRight } from 'lucide-react'
import { useCartStore } from '@/store/cart.store'
import { Button } from '@/components/ui/Button'
import { Input } from '@/components/ui/Input'
import { api } from '@/lib/api'

export function CartPage() {
  const { items, removeItem, updateQuantity, getTotal, clearCart } = useCartStore()
  const navigate = useNavigate()
  
  const [address, setAddress] = useState('')
  const [isCheckingOut, setIsCheckingOut] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const handleCheckout = async () => {
    if (items.length === 0) return
    setIsCheckingOut(true)
    setError(null)

    try {
      await api.post('/orders', {
        shippingAddress: address || 'Default Address',
        items: items.map(i => ({ productId: i.productId, quantity: i.quantity })),
        discountStrategy: 'None'
      })
      clearCart()
      navigate('/my-orders')
    } catch (err: any) {
      setError(err.response?.data?.error || 'Checkout failed.')
    } finally {
      setIsCheckingOut(false)
    }
  }

  if (items.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-20">
        <div className="rounded-full bg-slate-900 p-6 mb-6">
          <ShoppingBag className="h-16 w-16 text-slate-600" />
        </div>
        <h2 className="text-2xl font-bold text-slate-100 mb-2">Your cart is empty</h2>
        <p className="text-slate-400 mb-8">Looks like you haven't added any products yet.</p>
        <Button onClick={() => navigate('/catalog')}>Browse Catalog</Button>
      </div>
    )
  }

  return (
    <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
      <div className="lg:col-span-2 space-y-6">
        <div>
          <h1 className="text-3xl font-bold tracking-tight text-slate-100">Shopping Cart</h1>
          <p className="mt-1 text-slate-400">Review your items before checkout.</p>
        </div>

        <div className="rounded-xl border border-slate-800 bg-slate-900 overflow-hidden divide-y divide-slate-800">
          {items.map(item => (
            <div key={item.productId} className="flex items-center gap-4 p-6">
              <div className="h-20 w-20 rounded-md bg-slate-800 flex items-center justify-center border border-slate-700">
                <Package className="h-8 w-8 text-slate-500" />
              </div>
              <div className="flex-1 min-w-0">
                <h3 className="text-lg font-medium text-slate-200 truncate">{item.name}</h3>
                <p className="text-blue-400 font-bold">${item.price.toFixed(2)}</p>
              </div>
              <div className="flex items-center gap-3">
                <Input 
                  type="number" 
                  className="w-20 text-center" 
                  value={item.quantity}
                  onChange={(e) => updateQuantity(item.productId, parseInt(e.target.value) || 1)}
                />
                <Button variant="ghost" size="icon" onClick={() => removeItem(item.productId)}>
                  <Trash2 className="h-4 w-4 text-red-400" />
                </Button>
              </div>
            </div>
          ))}
        </div>
      </div>

      <div className="space-y-6">
        <div className="rounded-xl border border-slate-800 bg-slate-900 p-6 space-y-6">
          <h2 className="text-xl font-bold text-slate-100">Order Summary</h2>
          
          <div className="space-y-2 text-sm">
            <div className="flex justify-between text-slate-400">
              <span>Subtotal</span>
              <span>${getTotal().toFixed(2)}</span>
            </div>
            <div className="flex justify-between text-slate-400">
              <span>Shipping</span>
              <span>Free</span>
            </div>
            <div className="pt-4 flex justify-between border-t border-slate-800 text-lg font-bold text-slate-100">
              <span>Total</span>
              <span>${getTotal().toFixed(2)}</span>
            </div>
          </div>

          <div className="space-y-2">
            <label className="block text-sm font-medium text-slate-300">Shipping Address (Optional)</label>
            <Input 
              placeholder="Enter your full address" 
              value={address}
              onChange={e => setAddress(e.target.value)}
            />
          </div>

          {error && <div className="text-sm text-red-400 bg-red-400/10 p-3 rounded-md border border-red-400/20">{error}</div>}

          <Button 
            className="w-full gap-2" 
            size="lg" 
            onClick={handleCheckout}
            disabled={isCheckingOut}
          >
            {isCheckingOut ? 'Processing...' : 'Proceed to Checkout'} <ArrowRight className="h-4 w-4" />
          </Button>
        </div>
      </div>
    </div>
  )
}

function Package(props: any) {
  return (
    <svg
      {...props}
      xmlns="http://www.w3.org/2000/svg"
      width="24"
      height="24"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <path d="m7.5 4.27 9 5.15" />
      <path d="M21 8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16Z" />
      <path d="m3.3 7 8.7 5 8.7-5" />
      <path d="M12 22V12" />
    </svg>
  )
}
