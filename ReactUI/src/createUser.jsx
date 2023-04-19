import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

function CreateExercise() {
    const [name, setUName] = useState("");
    const [surname, setSurName] = useState("");
    const [email, setEmail] = useState("");
    const [phone, setPhone] = useState("");
    const [password, setPassword] = useState("Qwerty123!");
    const [subscription, setSub] = useState("");
    const navigate = useNavigate();

    const gotoUsers =() => {
        navigate("/Users");
    }

    let handleSubmit = async (e) => {
        e.preventDefault();
        try{
            let res = await fetch("http://localhost:5000/api/UsersControllerNew/CreateUser", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify({
                    name: name,
                    surname: surname,
                    email: email,
                    phone: phone,
                    password: password,
                }),
            });
            if(res.status === 200){
                gotoUsers();
            }
            if(res.status === 500){
                window.alert("An error occurred. Look into the console.");
                console.error("Create error", res);
            }
        } catch(err){
            window.alert("An error occurred. Look into the console.");
            console.error("Create error", err);
        }
    }

    return (
        <>
        <label>Name</label>
        <input type="text" id="name" onChange={(e) => setUName(e.target.value)} />
        <label>Surname</label>
        <input type="text" id="surname" onChange={(e) => setSurName(e.target.value)} />
        <label>Email</label>
        <input type="text" id="email" onChange={(e) => setEmail(e.target.value)} />
        <label>Phone</label>
        <input type="text" id="" onChange={(e) => setPhone(e.target.value)} />
        <label>Password</label>
        <input type="text" id="password" defaultValue="Qwerty123!" />
        <button onClick={handleSubmit}>Create</button>
        <button onClick={gotoUsers}>Cancel</button>
        </>
    )
}

export default CreateExercise;