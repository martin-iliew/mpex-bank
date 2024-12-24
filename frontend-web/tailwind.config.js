/** @type {import('tailwindcss').Config} */
import defaultTheme from 'tailwindcss/defaultTheme';
export default {
	darkMode: ["class"],
	content: ["./index.html", "./src/**/*.{ts,tsx,js,jsx}"],
	theme: {
	  extend: {
		borderRadius: {
		  lg: 'var(--radius)',
		  md: 'calc(var(--radius) - 2px)',
		  sm: 'calc(var(--radius) - 4px)',
		},
		colors: {
		  background: {
			DEFAULT: 'var(--background)',
			text: 'var(--text)',
			subtext: 'var(--subtext)',
			context: 'var(--context)',
		  },
		  foreground: {
			DEFAULT: 'var(--foreground)',
			text: 'var(--foreground-text)',
			subtext: 'var(--foreground-subtext)',
			context: 'var(--foreground-context)',
		  },
		  card: {
			DEFAULT: 'var(--card)',
			foreground: 'var(--card-foreground)',
		  },
		  popover: {
			DEFAULT: 'var(--popover)',
			foreground: 'var(--popover-foreground)',
		  },
		  primary: {
			DEFAULT: 'var(--primary)',
			foreground: 'var(--primary-foreground)',
			hover: 'var(--primary-hover)',
		  },
		  secondary: {
			DEFAULT: 'var(--secondary)',
			foreground: 'var(--secondary-foreground)',
			hover: 'var(--secondary-hover)', 
		  },
		  muted: {
			DEFAULT: 'var(--muted)',
			foreground: 'var(--muted-foreground)',
		  },
		  accent: {
			DEFAULT: 'var(--accent)',
			foreground: 'var(--accent-foreground)',
			hover: 'var(--accent-hover)', 
		  },
		  destructive: {
			DEFAULT: 'var(--destructive)',
			foreground: 'var(--destructive-foreground)',
		  },
		  border: 'var(--border)',
		  input: 'var(--input)',
		  ring: 'var(--ring)',
		  chart: {
			'1': 'var(--chart-1)',
			'2': 'var(--chart-2)',
			'3': 'var(--chart-3)',
			'4': 'var(--chart-4)',
			'5': 'var(--chart-5)',
		  }
		}
	  },
	  fontFamily: {
        sans: ['Recursive', ...defaultTheme.fontFamily.sans],
      }
	},
	plugins: [require("tailwindcss-animate")],
  }
  