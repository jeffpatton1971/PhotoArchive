import { Link } from 'react-router-dom';

function SiteHeader() {
  return (
    <header className="site-header">
      <h1>PhotoArchive</h1>
      <p>A personal photo archive frontend.</p>

      <nav className="site-nav">
        <Link to="/">Home</Link>
        <Link to="/archive">Archive</Link>
        <Link to="/on-this-day">On This Day</Link>
      </nav>
    </header>
  );
}

export default SiteHeader;