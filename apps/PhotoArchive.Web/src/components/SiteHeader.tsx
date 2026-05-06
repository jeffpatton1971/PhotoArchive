function SiteHeader() {
  return (
    <header className="site-header">
      <h1>PhotoArchive</h1>
      <p>A personal photo archive frontend.</p>

      <nav className="site-nav">
        <a href="/">Home</a>
        <a href="/archive">Archive</a>
        <a href="/on-this-day">On This Day</a>
      </nav>
    </header>
  );
}

export default SiteHeader;