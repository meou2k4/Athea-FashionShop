import { Outlet, NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';

const navItems = [
    { to: '/admin', label: 'Dashboard', icon: '📊', end: true },
    { to: '/admin/categories', label: 'Danh mục', icon: '🗂️' },
    { to: '/admin/products', label: 'Sản phẩm', icon: '👗' },
    { to: '/admin/properties', label: 'Màu & Kích thước', icon: '🎨' },
    { to: '/admin/settings', label: 'Cài đặt', icon: '⚙️' },
];

export default function AdminLayout() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/admin/login');
    };

    return (
        <div className="admin-layout">
            {/* Sidebar */}
            <aside className="sidebar">
                <div className="sidebar-logo">
                    <span>👗</span> FashionShop
                </div>
                <nav className="sidebar-nav">
                    {navItems.map((item) => (
                        <NavLink
                            key={item.to}
                            to={item.to}
                            end={item.end}
                            className={({ isActive }) => isActive ? 'active' : ''}
                        >
                            <span className="nav-icon">{item.icon}</span>
                            {item.label}
                        </NavLink>
                    ))}
                </nav>
                <div className="sidebar-footer">
                    <div style={{ fontSize: 12, color: 'var(--text-muted)', marginBottom: 10, padding: '0 4px' }}>
                        👤 {user?.fullName || user?.email}
                    </div>
                    <button className="logout-btn" onClick={handleLogout}>
                        🚪 Đăng xuất
                    </button>
                </div>
            </aside>

            {/* Main */}
            <div className="main-content">
                <div className="main-body">
                    <Outlet />
                </div>
            </div>
        </div>
    );
}
