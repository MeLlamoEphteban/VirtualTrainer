function showMenu() {
    const links = document.querySelector('.links');
    const user = document.querySelector('.nav-user');
        if (links.style.display=='flex' && user.style.display=='flex') {
            
            links.style.display = 'none'
            user.style.display = 'none'
        }
        else {
            links.style.display = 'flex'
            user.style.display = 'flex'
        }
};