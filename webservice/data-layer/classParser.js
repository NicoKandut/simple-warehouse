module.exports = (data, targetClass) => {
    return data.map(e => new targetClass(...e));
};