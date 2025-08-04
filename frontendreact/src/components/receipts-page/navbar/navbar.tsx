'use client'

import styles from "@/styles/recipts-page/navBar/navBar.module.css";
import { useRouter } from 'next/navigation';

export default function Navbar() {
    const router = useRouter();
    return (
        <header>
            <div className={styles.navBar}>
                <div className={styles.navItem}>
                    <h4 className={styles.TextHeader}>Склад</h4>
                    <a href="/receipts-page" className={styles.navLink}> Поступления</a>
                    <h4 className={styles.TextHeader}>Справочники</h4>
                    <a href="/unit-page" className={styles.navLink}>Единицы измерения</a>
                    <a href="/resources-page" className={styles.navLink}>Ресурсы</a>
                </div>
                
            </div>
        </header>
    )
}