import { Link } from "react-router-dom"
import { useRef, useState } from "react"
import axios from '../api/axios';
import useAuth from "../hooks/useAuth";
const FEED_URL = '/feed/random';
const POST_KWEET_URL = '/kweet/create';

const Profile = () => {
    const { auth } = useAuth();
    const errRef = useRef();
    const [errMsg, setErrMsg] = useState('');
    const [text, setText] = useState('');

    const getFeed = async (e) => {

        e.preventDefault();

        try {
            const response = await axios.get(FEED_URL);
            console.log(JSON.stringify(response?.data));
        } catch (err) {
            errRef.current.focus();
        }

    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(POST_KWEET_URL,
                JSON.stringify({ CustomerId: auth.id, Text: text }),
                {
                    headers: { 'Content-Type': 'application/json' }
                }
            );
            console.log(JSON.stringify(response));
            const roles = response?.data?.roles;
        } catch (err) {
            if (!err?.response) {
                setErrMsg('No Server Response');
            }
            errRef.current.focus();
        }
    }

    return (
        <section>
            <p ref={errRef} className={errMsg ? "errmsg" : "offscreen"} aria-live="assertive">{errMsg}</p>
            <h1>Profile</h1>
            <br />
            <p>Your profile</p>
            <form onSubmit={handleSubmit}>
                <label htmlFor="Text">Text:</label>
                <input
                    type="text"
                    id="text"
                    autoComplete="off"
                    onChange={(e) => setText(e.target.value)}
                    required
                />
                <button onClick={handleSubmit}>Post Kweet</button>
            </form>
            <div className="flexGrow">
                <button onClick={getFeed}>Get feed</button>
            </div>
            <div className="flexGrow">
                <Link to="/">Home</Link>
            </div>
        </section>
    )
}

export default Profile
