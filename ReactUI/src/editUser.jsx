import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

function EditUser(){
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);

    const [name, setUserName] = useState("");
    const [surname, setSurName] = useState("");
    const [phone, setPhone] = useState("");
    const [email, setEmail] = useState("");
    const [iduser, setUserID] = useState("");

    let { userID } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        fetch(`http://localhost:5000/api/UsersControllerNew/GetUserId/${userID}`)
        .then(res => res.json())
        .then(
            (result) => {
                setIsLoaded(true);
                setUserID(result.iduser);
                setUserName(result.name);
                setSurName(result.surname);
                setPhone(result.phone);
                setEmail(result.email);
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

    let handleSubmit = async (e) => {
        e.preventDefault();
        try{
            let res = await fetch("http://localhost:5000/api/UsersControllerNew/SaveUser", {
                method: "PUT",
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify({
                    iduser: iduser,
                    name: name,
                    surname: surname,
                    email: email,
                    phone: phone,
                }),
            });
            if(res.status === 200) {
                gotoUsers();
            }
        }catch(err) {
            console.log(err);
        }
    };
    
    if (error) {
    return <div>Error: {error.message}</div>;
    } else if (!isLoaded) {
    return <div>Loading...</div>;
    } else {
        return (
            <>
            <label>Name</label>
            <input type="text" id="name" defaultValue={name} onChange={(e) => setUserName(e.target.value)}/>
            <label>Surname</label>
            <input type="text" id="surname" defaultValue={surname} onChange={(e) => setSurName(e.target.value)}/>
            <label>Phone</label>
            <input type="text" id="phone" defaultValue={phone} onChange={(e) => setPhone(e.target.value)}/>
            <label>Email</label>
            <input type="text" id="email" defaultValue={email} onChange={(e) => setEmail(e.target.value)}/>
            <button onClick={handleSubmit}>Save</button>
            <button onClick={gotoUsers}>Cancel Edit</button>
            </>
        );
    }

}

export default EditUser;