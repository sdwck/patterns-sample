import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { Download, FileJson, FileText, TableProperties } from 'lucide-react'
import { api } from '@/lib/api'
import { Button } from '@/components/ui/Button'
import { useAuthStore } from '@/store/auth.store'

interface LowStockDto {
  id: string
  productName: string
  quantityOnHand: number
  reorderLevel: number
  warehouseLocation: string
}

export function ReportsPage() {
  const { token } = useAuthStore()
  const [isDownloading, setIsDownloading] = useState<string | null>(null)

  const { data: lowStock } = useQuery({
    queryKey: ['low-stock'],
    queryFn: async () => {
      const { data } = await api.get<LowStockDto[]>('/stock/low')
      return data
    }
  })

  const handleDownload = async (format: string) => {
    setIsDownloading(format)
    try {
      const response = await fetch(`/api/stock/report/export?format=${format}`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })
      
      if (!response.ok) throw new Error('Download failed')
      
      const blob = await response.blob()
      const contentDisposition = response.headers.get('Content-Disposition')
      let filename = `stock_report.${format}`
      
      if (contentDisposition) {
        const filenameMatch = contentDisposition.match(/filename="?([^"]+)"?/)
        if (filenameMatch && filenameMatch.length === 2) {
          filename = filenameMatch[1]
        }
      }

      const url = window.URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = filename
      document.body.appendChild(a)
      a.click()
      window.URL.revokeObjectURL(url)
      document.body.removeChild(a)
    } catch (error) {
      console.error(error)
    } finally {
      setIsDownloading(null)
    }
  }

  return (
    <div className="p-8 space-y-8">
      <div>
        <h1 className="text-3xl font-bold tracking-tight text-slate-100">Reports & Analytics</h1>
        <p className="mt-2 text-slate-400">Generate and download stock reports.</p>
      </div>

      <div className="grid gap-6 md:grid-cols-3">
        <div className="rounded-xl border border-slate-800 bg-slate-900 p-6 flex flex-col items-center text-center gap-4">
          <div className="rounded-full bg-blue-500/10 p-4">
            <TableProperties className="h-8 w-8 text-blue-400" />
          </div>
          <div>
            <h3 className="font-semibold text-slate-200">CSV Report</h3>
            <p className="text-sm text-slate-400 mt-1">Best for Excel and spreadsheet processing.</p>
          </div>
          <Button 
            className="w-full mt-auto" 
            onClick={() => handleDownload('csv')}
            disabled={!!isDownloading}
          >
            <Download className="mr-2 h-4 w-4" /> 
            {isDownloading === 'csv' ? 'Downloading...' : 'Export CSV'}
          </Button>
        </div>

        <div className="rounded-xl border border-slate-800 bg-slate-900 p-6 flex flex-col items-center text-center gap-4">
          <div className="rounded-full bg-amber-500/10 p-4">
            <FileJson className="h-8 w-8 text-amber-400" />
          </div>
          <div>
            <h3 className="font-semibold text-slate-200">JSON Report</h3>
            <p className="text-sm text-slate-400 mt-1">Machine-readable format for integrations.</p>
          </div>
          <Button 
            className="w-full mt-auto" 
            variant="outline"
            onClick={() => handleDownload('json')}
            disabled={!!isDownloading}
          >
            <Download className="mr-2 h-4 w-4" /> 
            {isDownloading === 'json' ? 'Downloading...' : 'Export JSON'}
          </Button>
        </div>

        <div className="rounded-xl border border-slate-800 bg-slate-900 p-6 flex flex-col items-center text-center gap-4">
          <div className="rounded-full bg-emerald-500/10 p-4">
            <FileText className="h-8 w-8 text-emerald-400" />
          </div>
          <div>
            <h3 className="font-semibold text-slate-200">Plain Text Report</h3>
            <p className="text-sm text-slate-400 mt-1">Human-readable summary format.</p>
          </div>
          <Button 
            className="w-full mt-auto" 
            variant="outline"
            onClick={() => handleDownload('txt')}
            disabled={!!isDownloading}
          >
            <Download className="mr-2 h-4 w-4" /> 
            {isDownloading === 'txt' ? 'Downloading...' : 'Export TXT'}
          </Button>
        </div>
      </div>

      <div className="rounded-xl border border-red-900/50 bg-red-950/10 overflow-hidden">
        <div className="px-6 py-4 border-b border-red-900/50 bg-red-900/20">
          <h3 className="font-semibold text-red-200">Low Stock Alerts</h3>
        </div>
        <div className="p-0">
          <table className="w-full text-sm text-left">
            <thead className="text-xs text-red-300 uppercase bg-transparent border-b border-red-900/30">
              <tr>
                <th className="px-6 py-3 font-medium">Product</th>
                <th className="px-6 py-3 font-medium">Current Stock</th>
                <th className="px-6 py-3 font-medium">Reorder Level</th>
                <th className="px-6 py-3 font-medium">Location</th>
              </tr>
            </thead>
            <tbody>
              {!lowStock?.length ? (
                <tr><td colSpan={4} className="px-6 py-4 text-slate-400">No items are running low on stock.</td></tr>
              ) : (
                lowStock.map((item) => (
                  <tr key={item.id} className="border-b border-red-900/20 last:border-0">
                    <td className="px-6 py-4 font-medium text-slate-200">{item.productName}</td>
                    <td className="px-6 py-4 text-red-400 font-bold">{item.quantityOnHand}</td>
                    <td className="px-6 py-4 text-slate-400">{item.reorderLevel}</td>
                    <td className="px-6 py-4 text-slate-400">{item.warehouseLocation || 'N/A'}</td>
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