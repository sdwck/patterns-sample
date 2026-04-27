import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { Package } from 'lucide-react'
import { api } from '@/lib/api'
import { useAuthStore } from '@/store/auth.store'
import { Input } from '@/components/ui/Input'
import { Button } from '@/components/ui/Button'

export function LoginPage() {
  const navigate = useNavigate()
  const setCredentials = useAuthStore((state) => state.setCredentials)
  
  const [email, setEmail] = useState('admin@warehouse.com')
  const[password, setPassword] = useState('admin123')
  const [error, setError] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    setIsLoading(true)

    try {
      const response = await api.post('/auth/login', { email, password })
      const { token, email: userEmail, role } = response.data
      
      setCredentials(token, userEmail, role)
      
      if (role === 'Admin' || role === 'Manager') {
        navigate('/admin/dashboard')
      } else {
        navigate('/catalog')
      }
    } catch (err: any) {
      setError(err.response?.data?.error || 'Invalid email or password.')
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className="flex min-h-screen items-center justify-center p-4">
      <div className="w-full max-w-md space-y-8 rounded-2xl bg-slate-900 p-8 shadow-xl border border-slate-800">
        <div className="flex flex-col items-center">
          <div className="rounded-full bg-blue-600/10 p-3 mb-4">
            <Package className="h-10 w-10 text-blue-500" />
          </div>
          <h2 className="text-2xl font-bold tracking-tight text-slate-100">
            Sign in to your account
          </h2>
          <p className="mt-2 text-sm text-slate-400">
            Or <Link to="/register" className="text-blue-400 hover:text-blue-300 transition-colors">create a new customer account</Link>
          </p>
        </div>

        <form className="space-y-6" onSubmit={handleSubmit}>
          {error && (
            <div className="rounded-md bg-red-500/10 p-4 border border-red-500/20 text-red-400 text-sm">
              {error}
            </div>
          )}
          
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-1">Email address</label>
              <Input
                type="email"
                required
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="admin@warehouse.com"
              />
            </div>
            
            <div>
              <label className="block text-sm font-medium text-slate-300 mb-1">Password</label>
              <Input
                type="password"
                required
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••"
              />
            </div>
          </div>

          <Button
            type="submit"
            className="w-full"
            disabled={isLoading}
          >
            {isLoading ? 'Signing in...' : 'Sign in'}
          </Button>
        </form>
      </div>
    </div>
  )
}
