import React from "react";
import { useAuth0 } from "@auth0/auth0-react";

function Home() {

    const { loginWithRedirect, logout, isAuthenticated } = useAuth0();

    return (
        <div>
            {isAuthenticated ? <button onClick={() => logout({ logoutParams: { returnTo: window.location.origin } })}>Logout</button> : <button onClick={() => loginWithRedirect()}>Login</button>}         
        </div>

    )
}

export default Home