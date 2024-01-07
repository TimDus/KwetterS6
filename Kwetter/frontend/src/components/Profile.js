import { Link } from "react-router-dom"
import { useRef, useState } from "react"
import axios from '../api/axios';
const FEED_URL = '/feed/random';

const Profile = () => {

    const errRef = useRef();
    const [errMsg, setErrMsg] = useState('');

    const getFeed = async (e) => {

        e.preventDefault();

        try {
            const response = await axios.get(FEED_URL);
            console.log(JSON.stringify(response?.data));
        } catch (err) {
            errRef.current.focus();
        }

    }

    return (
        <section>
            <p ref={errRef} className={errMsg ? "errmsg" : "offscreen"} aria-live="assertive">{errMsg}</p>
            <h1>Profile</h1>
            <br />
            <p>Your profile</p>
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
