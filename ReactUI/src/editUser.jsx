import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

function EditUser(){
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [item, setItem] = useState([]);

    let { userID } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        fetch(`http://localhost:5000/Users/GetUserId?id=${userID}`)
        .then(res => res.json())
        .then(
            (result) => {
                setIsLoaded(true);
                setItem(result);
            },
            (error) => {
                setIsLoaded(true);
                setError(error);
            }
        )
    }, [userID])

    const gotoUsers = () => {
        navigate("/Users");
    }
    
    if (error) {
    return <div>Error: {error.message}</div>;
    } else if (!isLoaded) {
    return <div>Loading...</div>;
    } else {
        return (
            <>
            <label>Name</label>
            <input type="text" id="name" value={item.name} />
            <label>Surname</label>
            <input type="text" id="surname" value={item.surname} />
            <label>Phone</label>
            <input type="text" id="phone" value={item.phone} />
            <label>Email</label>
            <input type="text" id="email" value={item.email} />
            <button onClick={gotoUsers}>Cancel Edit</button>
            </>
        );
    }

}

export default EditUser;