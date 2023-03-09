import React, { useState, useEffect } from "react";

function EditExercise({id, handleEdit}){
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [item, setItem] = useState([]);

    useEffect(() => {
        fetch(`http://localhost:5000/Exercises/GetExerciseId?id=${id}`)
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
    }, [id])

    if (error) {
    return <div>Error: {error.message}</div>;
  } else if (!isLoaded) {
    return <div>Loading...</div>;
  } else {
    return (
        <>
        <label>Exercise Name</label>
        <input type="text" id="name" value={item.exerciseName} />
        <label>Exercise Instructions</label>
        <input type="text" id="desc" value={item.instructions} />
        <button onClick={() => handleEdit(false)}>Cancel Edit</button>
        </>
    );
  }
}

export default EditExercise;