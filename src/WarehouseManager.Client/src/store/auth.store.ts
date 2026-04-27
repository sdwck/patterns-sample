import { create } from 'zustand'
import { persist } from 'zustand/middleware'

export type UserRole = 'Customer' | 'Manager' | 'Admin'

interface AuthState {
  token: string | null
  email: string | null
  role: UserRole | null
  setCredentials: (token: string, email: string, role: string) => void
  logout: () => void
  isAuthenticated: () => boolean
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      token: null,
      email: null,
      role: null,
      setCredentials: (token, email, role) => set({ token, email, role: role as UserRole }),
      logout: () => set({ token: null, email: null, role: null }),
      isAuthenticated: () => !!get().token,
    }),
    {
      name: 'auth-storage',
    }
  )
)