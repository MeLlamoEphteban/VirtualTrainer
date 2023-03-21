import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

function Row({item}) {
    const navigate = useNavigate();

    const handleEdit = (id) => {
        navigate("/Users/Edit/" + id);
    }

    return (
        <tr>
            <td>{item.name}</td>
            <td>{item.surname}</td>
            <td>{item.phone}</td>
            <td>{item.email}</td>
            <td>{item.userSubscription.idsubscriptionNavigation.subName}</td>
            <td>{item.userSubscription.endDate}</td>
            <td>
                <button onClick={() => handleEdit(item.iduser)}>Edit</button>
            </td>
        </tr>
    );
}

function MyComponent() {
    const [data, setData] = useState([]);

    useEffect(() => {
        fetch("http://localhost:5000/Users/GetUsersRaw")
        .then(res => res.json())
        .then(
            (result) => {
                setData(result);
            },
            (error) => {
                console.error(error);
            }
        )
}, [])

    return(
        <>
        <table>
        <thead>
            <tr>
            <th>Name</th>
            <th>Surname</th>
            <th>Phone</th>
            <th>Email</th>
            <th>Sub Name</th>
            <th>Expiry date</th>
            <th>Actions</th>
            </tr>
        </thead>
        <tbody>
        {data.map(item => (
          <Row key={item.iduser} item={item} />
        ))}
        </tbody>
        </table>
        </>
        );
    }

export default MyComponent;