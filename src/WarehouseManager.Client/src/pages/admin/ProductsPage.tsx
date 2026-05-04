import { useState, useEffect } from 'react'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { Plus, Edit2, Trash2, PackagePlus, FolderPlus } from 'lucide-react'
import { api } from '@/lib/api'
import { Button } from '@/components/ui/Button'
import { Input } from '@/components/ui/Input'
import { Modal } from '@/components/ui/Modal'

interface ProductDto {
    id: string
    name: string
    sku: string
    price: number
    categoryId: string
    categoryName: string
    stockQuantity: number
}

interface CategoryDto {
    id: string
    name: string
    parentCategoryId?: string | null
    subCategories?: CategoryDto[]
}

export function ProductsPage() {
    const queryClient = useQueryClient()
    const [isCreateOpen, setIsCreateOpen] = useState(false)
    const[isCategoryOpen, setIsCategoryOpen] = useState(false)
    const[isRestockOpen, setIsRestockOpen] = useState(false)
    const [isEditOpen, setIsEditOpen] = useState(false)
    const [selectedProductId, setSelectedProductId] = useState<string | null>(null)
    const[editingProduct, setEditingProduct] = useState<ProductDto | null>(null)
    const [search, setSearch] = useState('')

    const { data: productsData, isLoading } = useQuery({
        queryKey:['products', search],
        queryFn: async () => {
            const { data } = await api.get(`/products?search=${search}&pageSize=50`)
            return data
        }
    })

    const { data: categories } = useQuery({
        queryKey: ['categories'],
        queryFn: async () => {
            const { data } = await api.get<CategoryDto[]>('/categories')
            return data
        }
    })

    const deleteMutation = useMutation({
        mutationFn: (id: string) => api.delete(`/products/${id}`),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['products'] })
    })

    const openRestock = (id: string) => {
        setSelectedProductId(id)
        setIsRestockOpen(true)
    }

    const openEdit = (product: ProductDto) => {
        setEditingProduct(product)
        setIsEditOpen(true)
    }

    return (
        <div className="p-8 space-y-6">
            <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
                <h1 className="text-3xl font-bold tracking-tight text-slate-100">Products</h1>
                <div className="flex gap-3">
                    <Button onClick={() => setIsCategoryOpen(true)} variant="outline" className="gap-2">
                        <FolderPlus className="h-4 w-4" /> Add Category
                    </Button>
                    <Button onClick={() => setIsCreateOpen(true)} className="gap-2">
                        <Plus className="h-4 w-4" /> Add Product
                    </Button>
                </div>
            </div>

            <div className="flex gap-4 items-center">
                <Input
                    placeholder="Search by name or SKU..."
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                    className="max-w-md"
                />
            </div>

            <div className="rounded-md border border-slate-800 bg-slate-900 overflow-hidden">
                <div className="overflow-x-auto">
                    <table className="w-full text-sm text-left">
                        <thead className="text-xs text-slate-400 uppercase bg-slate-950/50 border-b border-slate-800">
                            <tr>
                                <th className="px-6 py-4 font-medium">Name</th>
                                <th className="px-6 py-4 font-medium">SKU</th>
                                <th className="px-6 py-4 font-medium">Category</th>
                                <th className="px-6 py-4 font-medium">Price</th>
                                <th className="px-6 py-4 font-medium">Stock</th>
                                <th className="px-6 py-4 font-medium text-right">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {isLoading ? (
                                <tr><td colSpan={6} className="px-6 py-8 text-center text-slate-400">Loading...</td></tr>
                            ) : productsData?.items?.length === 0 ? (
                                <tr><td colSpan={6} className="px-6 py-8 text-center text-slate-400">No products found.</td></tr>
                            ) : (
                                productsData?.items?.map((p: ProductDto) => (
                                    <tr key={p.id} className="border-b border-slate-800/50 hover:bg-slate-800/20 transition-colors">
                                        <td className="px-6 py-4 font-medium text-slate-200">{p.name}</td>
                                        <td className="px-6 py-4 text-slate-400">{p.sku}</td>
                                        <td className="px-6 py-4">
                                            <span className="inline-flex items-center rounded-md bg-slate-800 px-2 py-1 text-xs font-medium text-slate-300 ring-1 ring-inset ring-slate-700">
                                                {p.categoryName}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4">${p.price.toFixed(2)}</td>
                                        <td className="px-6 py-4">
                                            <span className={p.stockQuantity <= 10 ? 'text-red-400 font-medium' : 'text-green-400 font-medium'}>
                                                {p.stockQuantity}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4 flex items-center justify-end gap-2">
                                            <Button variant="ghost" size="icon" onClick={() => openRestock(p.id)} title="Restock">
                                                <PackagePlus className="h-4 w-4 text-blue-400" />
                                            </Button>
                                            <Button variant="ghost" size="icon" onClick={() => openEdit(p)} title="Edit">
                                                <Edit2 className="h-4 w-4 text-slate-400" />
                                            </Button>
                                            <Button variant="ghost" size="icon" onClick={() => deleteMutation.mutate(p.id)} title="Delete">
                                                <Trash2 className="h-4 w-4 text-red-400" />
                                            </Button>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
            </div>

            <CreateProductModal
                isOpen={isCreateOpen}
                onClose={() => setIsCreateOpen(false)}
                categories={categories ||[]}
            />

            <CreateCategoryModal
                isOpen={isCategoryOpen}
                onClose={() => setIsCategoryOpen(false)}
                categories={categories ||[]}
            />

            {selectedProductId && (
                <RestockModal
                    isOpen={isRestockOpen}
                    onClose={() => {
                        setIsRestockOpen(false)
                        setSelectedProductId(null)
                    }}
                    productId={selectedProductId}
                />
            )}

            {editingProduct && (
                <EditProductModal
                    isOpen={isEditOpen}
                    onClose={() => {
                        setIsEditOpen(false)
                        setEditingProduct(null)
                    }}
                    product={editingProduct}
                    categories={categories ||[]}
                />
            )}
        </div>
    )
}

function CreateCategoryModal({ isOpen, onClose, categories }: { isOpen: boolean, onClose: () => void, categories: CategoryDto[] }) {
    const queryClient = useQueryClient()
    const [formData, setFormData] = useState({ name: '', description: '', parentCategoryId: '' })
    const [error, setError] = useState<string | null>(null)

    const mutation = useMutation({
        mutationFn: (data: any) => api.post('/categories', data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['categories'] })
            onClose()
            setFormData({ name: '', description: '', parentCategoryId: '' })
        },
        onError: (err: any) => setError(err.response?.data?.error || 'Creation failed')
    })

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault()
        setError(null)
        
        mutation.mutate({
            name: formData.name,
            description: formData.description,
            parentCategoryId: formData.parentCategoryId || null 
        })
    }

    return (
        <Modal isOpen={isOpen} onClose={onClose} title="Create Category">
            <form onSubmit={handleSubmit} className="space-y-4">
                {error && <div className="text-red-400 text-sm bg-red-400/10 p-3 rounded">{error}</div>}
                
                <div>
                    <label className="block text-sm font-medium text-slate-300 mb-1">Name</label>
                    <Input required value={formData.name} onChange={e => setFormData({ ...formData, name: e.target.value })} />
                </div>
                
                <div>
                    <label className="block text-sm font-medium text-slate-300 mb-1">Description</label>
                    <Input value={formData.description} onChange={e => setFormData({ ...formData, description: e.target.value })} />
                </div>
                
                <div>
                    <label className="block text-sm font-medium text-slate-300 mb-1">Parent Category</label>
                    <select
                        className="flex h-10 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-slate-100 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-blue-500"
                        value={formData.parentCategoryId}
                        onChange={e => setFormData({ ...formData, parentCategoryId: e.target.value })}
                    >
                        <option value="">None (Root Category)</option>
                        {flattenCategories(categories).map(c => (
                            <option key={c.id} value={c.id}>
                                {'— '.repeat(c.level) + c.name}
                            </option>
                        ))}
                    </select>
                </div>
                
                <div className="flex justify-end gap-3 pt-4">
                    <Button type="button" variant="ghost" onClick={onClose}>Cancel</Button>
                    <Button type="submit" disabled={mutation.isPending}>Create</Button>
                </div>
            </form>
        </Modal>
    )
}

function CreateProductModal({ isOpen, onClose, categories }: { isOpen: boolean, onClose: () => void, categories: CategoryDto[] }) {
    const queryClient = useQueryClient()
    const [formData, setFormData] = useState({ name: '', sku: '', price: '', categoryId: '', initialStock: '0' })
    const [error, setError] = useState<string | null>(null)

    const mutation = useMutation({
        mutationFn: (data: any) => api.post('/products', data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['products'] })
            onClose()
            setFormData({ name: '', sku: '', price: '', categoryId: '', initialStock: '0' })
        },
        onError: (err: any) => setError(err.response?.data?.error || 'Creation failed')
    })

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault()
        setError(null)
        mutation.mutate({
            ...formData,
            price: parseFloat(formData.price),
            initialStock: parseInt(formData.initialStock, 10),
            categoryId: formData.categoryId || categories[0]?.id
        })
    }

    return (
        <Modal isOpen={isOpen} onClose={onClose} title="Create New Product">
            <form onSubmit={handleSubmit} className="space-y-4">
                {error && <div className="text-red-400 text-sm bg-red-400/10 p-3 rounded">{error}</div>}
                <div>
                    <label className="block text-sm font-medium text-slate-300 mb-1">Name</label>
                    <Input required value={formData.name} onChange={e => setFormData({ ...formData, name: e.target.value })} />
                </div>
                <div>
                    <label className="block text-sm font-medium text-slate-300 mb-1">SKU</label>
                    <Input required value={formData.sku} onChange={e => setFormData({ ...formData, sku: e.target.value })} />
                </div>
                <div className="grid grid-cols-2 gap-4">
                    <div>
                        <label className="block text-sm font-medium text-slate-300 mb-1">Price</label>
                        <Input type="number" step="0.01" required value={formData.price} onChange={e => setFormData({ ...formData, price: e.target.value })} />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-slate-300 mb-1">Initial Stock</label>
                        <Input type="number" required value={formData.initialStock} onChange={e => setFormData({ ...formData, initialStock: e.target.value })} />
                    </div>
                </div>
                <div>
                    <label className="block text-sm font-medium text-slate-300 mb-1">Category</label>
                    <select
                        className="flex h-10 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-slate-100 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-blue-500"
                        value={formData.categoryId}
                        onChange={e => setFormData({ ...formData, categoryId: e.target.value })}
                        required
                    >
                        <option value="" disabled>Select a category</option>
                        {flattenCategories(categories).map(c => (
                            <option key={c.id} value={c.id}>
                                {'— '.repeat(c.level) + c.name}
                            </option>
                        ))}
                    </select>
                </div>
                <div className="flex justify-end gap-3 pt-4">
                    <Button type="button" variant="ghost" onClick={onClose}>Cancel</Button>
                    <Button type="submit" disabled={mutation.isPending}>Create</Button>
                </div>
            </form>
        </Modal>
    )
}

function EditProductModal({
    isOpen,
    onClose,
    product,
    categories
}: {
    isOpen: boolean
    onClose: () => void
    product: ProductDto
    categories: CategoryDto[]
}) {
    const queryClient = useQueryClient()
    const [formData, setFormData] = useState({
        name: '',
        price: '',
        categoryId: ''
    })

    useEffect(() => {
        setFormData({
            name: product.name,
            price: product.price.toString(),
            categoryId: product.categoryId
        })
    },[product])

    const mutation = useMutation({
        mutationFn: () =>
            api.put(`/products/${product.id}`, {
                id: product.id,
                name: formData.name,
                price: parseFloat(formData.price),
                categoryId: formData.categoryId
            }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['products'] })
            onClose()
        }
    })

    return (
        <Modal isOpen={isOpen} onClose={onClose} title="Edit Product">
            <form
                onSubmit={e => {
                    e.preventDefault()
                    mutation.mutate()
                }}
                className="space-y-4"
            >
                <div>
                    <label htmlFor="name" className="block text-sm text-slate-300 mb-1">
                        Name
                    </label>
                    <Input
                        id="name"
                        value={formData.name}
                        onChange={e => setFormData({ ...formData, name: e.target.value })}
                    />
                </div>

                <div>
                    <label htmlFor="price" className="block text-sm text-slate-300 mb-1">
                        Price
                    </label>
                    <Input
                        id="price"
                        type="number"
                        value={formData.price}
                        onChange={e => setFormData({ ...formData, price: e.target.value })}
                    />
                </div>

                <div>
                    <label htmlFor="category" className="block text-sm text-slate-300 mb-1">
                        Category
                    </label>
                    <select
                        id="category"
                        value={formData.categoryId}
                        onChange={e => setFormData({ ...formData, categoryId: e.target.value })}
                        className="flex h-10 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-slate-100"
                    >
                        {flattenCategories(categories).map(c => (
                            <option key={c.id} value={c.id}>
                                {'— '.repeat(c.level) + c.name}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="flex justify-end gap-2 pt-4">
                    <Button type="button" variant="ghost" onClick={onClose}>Cancel</Button>
                    <Button type="submit" disabled={mutation.isPending}>Save</Button>
                </div>
            </form>
        </Modal>
    )
}

function RestockModal({ isOpen, onClose, productId }: { isOpen: boolean, onClose: () => void, productId: string }) {
    const queryClient = useQueryClient()
    const [quantity, setQuantity] = useState('10')

    const mutation = useMutation({
        mutationFn: (qty: number) => api.post('/stock/restock', { productId, quantity: qty }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey:['products'] })
            queryClient.invalidateQueries({ queryKey: ['dashboard-stats'] })
            onClose()
        }
    })

    return (
        <Modal isOpen={isOpen} onClose={onClose} title="Restock Product">
            <form onSubmit={e => { e.preventDefault(); mutation.mutate(parseInt(quantity, 10)) }} className="space-y-4">
                <div>
                    <label className="block text-sm font-medium text-slate-300 mb-1">Quantity to add</label>
                    <Input type="number" required value={quantity} onChange={e => setQuantity(e.target.value)} />
                </div>
                <div className="flex justify-end gap-3 pt-4">
                    <Button type="button" variant="ghost" onClick={onClose}>Cancel</Button>
                    <Button type="submit" disabled={mutation.isPending}>Add Stock</Button>
                </div>
            </form>
        </Modal>
    )
}

function flattenCategories(categories: any[], level = 0): any[] {
    return categories.flatMap(c =>[
        { ...c, level },
        ...flattenCategories(c.subCategories || [], level + 1)
    ])
}