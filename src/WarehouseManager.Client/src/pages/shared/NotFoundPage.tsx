import { Link, useRouteError } from 'react-router-dom'
import { AlertTriangle, ArrowLeft } from 'lucide-react'
import { Button } from '@/components/ui/Button'

export function NotFoundPage() {
  const error: any = useRouteError()

  const status = error?.status || 404
  const message =
    error?.statusText ||
    error?.message ||
    'Page not found'

  return (
    <div className="min-h-[70vh] flex items-center justify-center px-4">
      <div className="w-full max-w-md text-center space-y-6">
        <div className="flex justify-center">
          <div className="rounded-full bg-red-500/10 p-4">
            <AlertTriangle className="h-10 w-10 text-red-400" />
          </div>
        </div>

        <div>
          <h1 className="text-4xl font-bold text-slate-100">{status}</h1>
          <p className="mt-2 text-lg text-slate-300">{message}</p>
          <p className="mt-1 text-sm text-slate-500">
            The page you’re looking for doesn’t exist or an unexpected error occurred.
          </p>
        </div>

        <div className="flex flex-col sm:flex-row gap-3 justify-center">
          <Button variant="outline">
            <Link to="/" className="flex items-center gap-2">
              <ArrowLeft className="h-4 w-4" />
              Go Home
            </Link>
          </Button>

          <Button
            onClick={() => window.location.reload()}
            className="gap-2"
          >
            Refresh
          </Button>
        </div>

        {error?.stack && (
          <details className="text-left text-xs text-slate-500 bg-slate-900 border border-slate-800 rounded-lg p-3">
            <summary className="cursor-pointer">Error details</summary>
            <pre className="mt-2 whitespace-pre-wrap">{error.stack}</pre>
          </details>
        )}
      </div>
    </div>
  )
}