import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { Search, ShoppingCart } from 'lucide-react'
import { api } from '@/lib/api'
import { Button } from '@/components/ui/Button'
import { Input } from '@/components/ui/Input'
import { useCartStore } from '@/store/cart.store'

interface ProductDto {
  id: string
  name: string
  description: string
  price: number
  categoryName: string
  stockQuantity: number
}

export function CatalogPage() {
  const [search, setSearch] = useState('')
  const addItem = useCartStore(state => state.addItem)

  const { data, isLoading } = useQuery({
    queryKey: ['public-products', search],
    queryFn: async () => {
      const { data } = await api.get(`/products?search=${search}&pageSize=50&onlyInStock=true`)
      return data
    }
  })

  return (
    <div className="space-y-8">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight text-slate-100">Catalog</h1>
          <p className="mt-1 text-slate-400">Discover our range of products.</p>
        </div>
        <div className="relative w-full sm:w-72">
          <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <Search className="h-4 w-4 text-slate-500" />
          </div>
          <Input 
            className="pl-10" 
            placeholder="Search products..." 
            value={search}
            onChange={e => setSearch(e.target.value)}
          />
        </div>
      </div>

      {isLoading ? (
        <div className="text-slate-400 animate-pulse">Loading products...</div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          {data?.items?.length === 0 ? (
            <div className="col-span-full py-12 text-center text-slate-400">No products found.</div>
          ) : (
            data?.items?.map((p: ProductDto) => (
              <div key={p.id} className="group flex flex-col rounded-xl border border-slate-800 bg-slate-900 p-5 shadow-sm hover:border-slate-700 transition-colors">
                <div className="flex-1">
                  <div className="flex items-center justify-between mb-2">
                    <span className="inline-flex items-center rounded-md bg-blue-500/10 px-2 py-1 text-xs font-medium text-blue-400 ring-1 ring-inset ring-blue-500/20">
                      {p.categoryName}
                    </span>
                    <span className="text-xs text-slate-500">{p.stockQuantity} in stock</span>
                  </div>
                  <h3 className="text-lg font-semibold text-slate-100 leading-tight mb-1">{p.name}</h3>
                  <p className="text-sm text-slate-400 line-clamp-2">{p.description || 'No description available.'}</p>
                </div>
                <div className="mt-4 flex items-center justify-between pt-4 border-t border-slate-800">
                  <span className="text-xl font-bold text-slate-100">${p.price.toFixed(2)}</span>
                  <Button 
                    size="sm" 
                    className="gap-2"
                    onClick={() => addItem({ productId: p.id, name: p.name, price: p.price, quantity: 1 })}
                  >
                    <ShoppingCart className="h-4 w-4" /> Add
                  </Button>
                </div>
              </div>
            ))
          )}
        </div>
      )}
    </div>
  )
}
