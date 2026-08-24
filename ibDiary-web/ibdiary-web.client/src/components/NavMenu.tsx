function NavMenu() {
  return (
      <div className="nav-menu-container">
          <div>
              <a href="/">
                  <h1>IbDiary</h1>
              </a>
          </div>
          <div>
              <a href="/features">
                <a>Features</a>
              </a>
              <a href="/how-it-works">
                <p>How it Works</p>
              </a>
              <a href="/alpha">
                <p>Alpha</p>
              </a>
          </div>
          <div>
              <a href="/join-alpha">
                <p>Join Alpha!</p>
              </a>
          </div>
      </div>
  );
}

export default NavMenu;