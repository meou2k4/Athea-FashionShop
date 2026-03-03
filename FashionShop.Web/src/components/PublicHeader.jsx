import { Link, useLocation } from 'react-router-dom';


export default function PublicHeader() {
    const location = useLocation();

    const filter = new URLSearchParams(location.search).get('filter');
    const isProductPage = location.pathname === '/san-pham';

    const navClass = (active) => active ? 'active' : '';

    return (
        <header className="pub-header">
            <div className="container">
                <Link to="/" className="pub-logo">
                    ATHEA<span>.</span>
                </Link>
                <nav className="pub-nav">
                    <Link to="/" className={navClass(location.pathname === '/')}>Trang chủ</Link>
                    <Link to="/san-pham" className={navClass(isProductPage && !filter)}>Tất cả sản phẩm</Link>
                    <Link to="/san-pham?filter=new" className={navClass(isProductPage && filter === 'new')}>Sản phẩm mới</Link>
                    <Link to="/san-pham?filter=sale" className={navClass(isProductPage && filter === 'sale')}>Sản phẩm giảm giá</Link>
                </nav>
            </div>
        </header>
    );
}

